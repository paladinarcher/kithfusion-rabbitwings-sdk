using System.Collections.Generic;
using UnityEngine;
using Xsolla.Auth;
using Xsolla.Core;
using Xsolla.Inventory;
using Xsolla.Catalog;
using System;
using UnityEngine.Events;
using static Xsolla.Core.UserInfo;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;
using System.Runtime.Serialization;
using System.Collections;
using Xsolla.UserAccount;
using static UnityEngine.ParticleSystem;
using System.Collections.Concurrent;
using Error = Xsolla.Core.Error;
using System.Linq;
//5zXOQ4cVnCUXqsRqGOk6M8WROodlLPqk

public class AuthManager : MonoBehaviour
{
    public enum ActionType
    {
        BuyFree,
        BuyVirtual,
        SellForRIS,
        Consume
    }
    [Serializable]
    public class ActionPack
    {
        public InventoryItem item;
        public ActionType type;
        public BundleItem bundle;
        public StoreItem storeItem;

        public event Action<OrderId> onSuccessOrderId;
        public event Action<Error> onError;
        public event Action onSuccess;

        private int quantity = 0;
        public string Index
        {
            get
            {
                if (item != null) { return item.sku; }
                if (bundle != null) { return bundle.sku; }
                if (storeItem != null) { return storeItem.sku; }
                return "";
            }
        }
        public int Quantity
        {
            get
            {
                if (item != null) { return item.quantity; }
                return quantity;
            }
            set
            {
                if (item != null) { item.quantity = value; }
                else
                {
                    quantity = value;
                }
            }
        }
        public Action OnSuccess
        {
            get
            {
                return () => {
                    onSuccess.Invoke();
                };
            }
            set
            {
                onSuccess += value;
            }
        }
        public Action<OrderId> OnSuccessOrderId
        {
            get
            {
                return (OrderId o) => {
                    onSuccessOrderId.Invoke(o);
                };
            }
            set
            {
                onSuccessOrderId += value;
            }
        }
        public Action<Error> OnError
        {
            get
            {
                return (Error o) => {
                    onError.Invoke(o);
                }; 
            }
            set
            {
                onError += value;
            }
        }
    }
    // Declaration of variables for UI elements on the page
    public const string ITEM_GROUP_NOT_FOR_SALE = "nosell";
    public const string ATTRIBUTE_INVENTORY_SHOW = "inventory_show";
    public const string ATTRIBUTE_GOAL_ITEM = "goal_item";
    public const string ATTRIBUTE_PHYSICAL_ITEM = "physical_item";
    public const string ATTRIBUTE_GOAL_ITEM_ORDER = "goal_item_order";
    public const string ATTRIBUTE_INVENTORY_SHOW_VALUE = "yes";
    public const string CURRENCY_VALUE_ATTRIBUTE_ID = "ris_credit_sell_value";
    public const string RIS_CURRENCY_SKU = "ris-credits";
    public const string RIS_CURRENCY_VALUE_PREFIX = "ris_";
    public const string GOAL_ITEM_PREFIX = "goal_";
    public const string DEFAULT_DEBUG_PASSWORD = "asdf1234";
    public Dictionary<string, string> VCURENCY_SKU_TO_NAME;
    public const string VCURRENCY_PACKAGE_SKU = "RISCurrency";
    //public const string BASE_STORE_API_URL = "https://store.xsolla.com/api/v2/project/{0}";
    //public const string URL_FREE_ITEM = Constants.BASE_STORE_API_URL + "/free/item/{1}";
    public UnityEvent<string> onError;
    public UnityEvent onSuccessfulLogin;
    public AuthInventoryItemEvent inventoryPopulated;
    public AuthInventoryItemEvent balancesPopulated;
    public InventoryItemEvent onAddSuccess;
    public InventoryItemEvent onSpriteLoad;
    public InventoryItemEvent onItemSold;
    public UnityIntEvent risChange;
    public float minimumWaitAfterWrite = 2f;
    public float LastWrite
    {
        get { return lastWrite; }
    }
    public bool RelogginngIn { get { return relogin; } }

    private bool pullingInventory = false;
    private bool waitingInventory = false;
    private float lastWrite = 0f;
    private Coroutine refreshingCoroutine;
    private Coroutine batchItems;
    private ConcurrentDictionary<string, ActionPack> batchedItems = new ConcurrentDictionary<string, ActionPack>();
    private float waitBetweenBatches = 30f;
    private float waitBetweenItems = 5f;
    private bool relogin = false;

    private void Awake()
    {
        VCURENCY_SKU_TO_NAME = new Dictionary<string, string>
        {
            { "ris-credits", "RIS Credits" }
        };
    }

    [Serializable]
    public class XsollaUser {
        public string email;
        public string password;
        public string loginToken;
        public List<VirtualCurrencyBalance> vcurrencyBalances;
        public List<InventoryItem> inventoryItems;
        public GoalItemManager goalItems;
        public GoalManager.GoalStateSummary goalSummary;
        public string id;
        public string username;
        public string nickname;
        public string name;
        public string picture;
        public string birthday;
        public string first_name;
        public string last_name;
        public string gender;
        public string phone;
        public List<UserGroup> groups;
        public string registered;
        public string external_id;
        public string last_login;
        public UserBan ban;
        public string country;
        public string tag;
        [CanBeNull] public string connection_information;
        [CanBeNull] public bool? is_anonymous;
        [CanBeNull] public string phone_auth;
        /// <summary>
        /// User status. Can be 'online' or 'offline'.
        /// </summary>
        [CanBeNull] public string presence;
    }
    public class InventorySellDescriptor
    {
        public InventoryItem item;
        public bool isSellable = false;
        public string currenySku = RIS_CURRENCY_SKU;
        public string currenctName;
        public int exchangeRate = 0;
    }
    [Serializable]
    public class AuthInventoryItemEvent : UnityEvent<XsollaUser>
    {

    }

    [Serializable]
    public class AuthCatelogItemEvent : UnityEvent<BundleItem> { }

    public XsollaUser userObj = new XsollaUser();

    public void initiateSignIn(string email, string password, Action onSuccess, Action<Error> onSigninError) {
        userObj.email = email;
        if(!GameController.IsProdDB && string.IsNullOrEmpty(password)) { password = DEFAULT_DEBUG_PASSWORD; }
        userObj.password = password;
        GameController.AddLog("AuthManager", $"signing in {email}");
        XsollaAuth.SignIn(email, password, onSuccess: () => { 
                relogin = false;
            }, onError: (Error e) => {
                OnError(e);
                //onSigninError.Invoke(e);
                relogin = false;
            });
        }, onError:(Error e) => { 
            OnError(e);
            onSigninError.Invoke(e);
            relogin = false;
        });
    }

    public void RefreshUserData(Action onSuccess, Action<Error> onError)
    {
        XsollaAuth.GetUserInfo((UserInfo u) => {
            OnGetUserInfoSuccess(u, () => { });
            PopulateInventory((InventoryItems itm) => {
                onSuccess?.Invoke();
            });
        }, onError: (Error e) => {
            OnError(e);
            onError.Invoke(e);
        });
    }

    public void initiateSignIn(string email, string password, Action onSuccess, Action onCreateError, Action onUpdateError)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            var error = new Error("400", "Parâmetros inválidos", "Email ou senha não podem estar vazios");
            onCreateError?.Invoke(error);
            return;
        }

        userObj.email = email;
        userObj.password = password;
        GameController.AddLog("AuthManager", $"signing in {userObj.email}");

        Auth.SignIn(() =>
        {
            Debug.Log($"Loggedin via Device");
            XsollaUserAccount.AddUsernameEmailAuthToAccount(userObj.password, userObj.email,
                (AddUsernameAndEmailResult added) =>
                {
                    Debug.Log($"Added {userObj.password}, {userObj.email} to account.");
                },
                (Error e) =>
                {
                    relogin = false;

                    // Mapeamento dos novos códigos de erro
                    switch (e.statusCode)
                    {
                        case "404":
                            Debug.LogError($"Usuário {userObj.email} não encontrado!");
                            break;
                        case "401":
                            Debug.LogError($"Credenciais inválidas para o usuário {userObj.email}!");
                            relogin = true;
                            break;
                        case "400":
                            Debug.LogError($"Parâmetros inválidos para o usuário {userObj.email}!");
                            break;
                        case "500":
                            Debug.LogError($"Erro do servidor ao processar a requisição para {userObj.email}!");
                            break;
                        default:
                            Debug.LogError($"Erro desconhecido: {e.statusCode}");
                            break;
                    }

                    XsollaAuth.Logout(() =>
                    {
                        Debug.Log($"Successfully backed out since we couldn't update the user with details given.");
                        if (relogin)
                        {
                            initiateSignIn(email, password, onSuccess, onUpdateError);
                        }
                    },
                    (Error er) =>
                    {
                        Debug.LogError($"Error logging out: {er}");
                    });

                    Debug.LogError(e);
                    OnError(e);
                    onUpdateError?.Invoke(e);
                });
        },
        (Error e) =>
        {
            Debug.LogError(e);
            OnError(e);
            onCreateError?.Invoke(e);
        });
    }

    private void OnSignInSuccess()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventLogin,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterMethod, "XSolla"),
                new Firebase.Analytics.Parameter("device_id", SystemInfo.deviceUniqueIdentifier)
            }
        );
        //Debug.Log($"Authorization successful. Token: {token}");
        GameController.AddLog("AuthManager", $"Authorization successful. Token: no longer needed");
        //userObj.loginToken = token;
        if(onSuccessfulLogin != null)
        {
            onSuccessfulLogin.Invoke();
        }
        //Token.Instance = Token.Create(token);
        //XsollaCatalog.Instance.GetCatalog(XsollaSettings.StoreProjectId, OnItemsRequestSuccess, OnError, offset: 0, limit: 50);
    }

    public void PopulateVirtualCurrencies()
    {
        XsollaInventory.GetVirtualCurrencyBalance(OnBalanceRequestSuccess, OnError);
    }
    public void PopulateInventory()
    {
        PopulateInventory(0f, null);
    }
    public void PopulateInventory(float wait)
    {
        PopulateInventory(wait, null);
    }
    public void PopulateInventory(Action<InventoryItems> onSuccess)
    {
        PopulateInventory(0f, onSuccess);
    }
    public void PopulateInventory(float wait, Action<InventoryItems> onSuccess)
    {
        if(refreshingCoroutine != null && waitingInventory)
        {
            StopCoroutine(refreshingCoroutine);
            pullingInventory = false;
            waitingInventory = false;
        }
        refreshingCoroutine = StartCoroutine(PauseRefresh(wait, onSuccess));
    }
    public void purchaseItem(BundleItem item, Action<OrderId> onSuccessOrderId, Action<Error> onError) { purchaseItem(item, 1, onSuccessOrderId, onError); }
    public void purchaseItem(BundleItem item, int quantity, Action<OrderId> onSuccessOrderId, Action<Error> onError)
    {
        doBundle(item, quantity, ActionType.BuyVirtual, onSuccessOrderId, () => { }, onError);
    }
    public void purchaseItem(InventoryItem item, Action<OrderId> onSuccessOrderId, Action<Error> onError)
    {
        doItem(item, ActionType.BuyVirtual, onSuccessOrderId, () => { }, onError);
    }
    private void purchaseItemSingle(InventoryItem item, int quantity, Action<OrderId> onSuccess, Action<Error> onError)
    {
        VirtualPrice price = GameController.Instance.itemCache.GetRISPrice(item);
        if (price == null)
        {
            Debug.LogError($"Price is null: {item} {GameController.Instance.itemCache.GetStoreBySKU(item)}");
            onError.Invoke(new Error() { errorMessage = $"Price is null: {item} {GameController.Instance.itemCache.GetStoreBySKU(item)}" });
        }
        else
        {
            XsollaCatalog.CreateOrderByVirtualCurrency(item.sku, price.sku, (OrderId p) =>
            {
                GameController.Instance.itemCache.Refresh();
                onSuccess?.Invoke(p);
            }, (e) => { Debug.LogError(e.errorMessage); onError?.Invoke(e); }, new PurchaseParams() { quantity= (int?)quantity });
        }
    }
    private void purchaseBundleSingle(BundleItem item, int quantity, Action<OrderId> onSuccess, Action<Error> onError)
    {
        VirtualPrice price = item.virtual_prices.First(x => x.is_default);
        if (price == null)
        {
            Debug.LogError($"Price is null: {item} ");
            onError.Invoke(new Error() { errorMessage = $"Price is null: {item}" });
        }
        else
        {
            XsollaCatalog.CreateOrderByVirtualCurrency(item.sku, price.sku, (OrderId p) =>
            {
                GameController.Instance.itemCache.Refresh();
                onSuccess?.Invoke(p);
            }, (e) => { Debug.LogError(e.errorMessage); onError?.Invoke(e); }, new PurchaseParams() { quantity=(int?)quantity });
        }
    }
    public void purchaseFreeItem(InventoryItem item, Action<OrderId> onSuccessOrderId, Action<Error> onError) {
        doItem(item, ActionType.BuyFree, onSuccessOrderId, () => { }, onError);
    }
    public void doItem(ActionPack ap)
    {
        string key = (ap.item != null ? ap.item.sku : (ap.bundle != null ? ap.bundle.sku : "NO ITEM OR BUNDLE?"))+ap.type.ToString();
        if(batchedItems.ContainsKey(key))
        {
            batchedItems[key].Quantity += ap.Quantity;
            batchedItems[key].OnSuccessOrderId = ap.OnSuccessOrderId;
            batchedItems[key].OnSuccess = ap.OnSuccess;
            batchedItems[key].OnError = ap.OnError;
        } else
        {
            batchedItems[key] = ap;
        }

        if (batchItems == null)
        {
            batchItems = StartCoroutine(RunBatches());
        }
    }
    public void doItem(InventoryItem item, ActionType type, Action<OrderId> onSuccessOrderId, Action onSuccess, Action<Error> onError)
    {
        ActionPack ap = new ActionPack
        {
            item = item,
            type = type,
            OnError = onError,
            OnSuccess = onSuccess,
            OnSuccessOrderId = onSuccessOrderId
        };
        doItem(ap);
    }
    public void doBundle(BundleItem item, int quantity, ActionType type, Action<OrderId> onSuccessOrderId, Action onSuccess, Action<Error> onError)
    {
        ActionPack ap = new ActionPack
        {
            bundle = item,
            Quantity = quantity,
            type = type,
            OnError = onError,
            OnSuccess = onSuccess,
            OnSuccessOrderId = onSuccessOrderId
        };
        doItem(ap);
    }
    private IEnumerator RunBatches() {
        float originalWait = waitBetweenItems;
        while(true)
        {
            //Debug.Log("Starting processing the batch.");
            while(batchedItems.Count > 0)
            {
                //Debug.Log("Starting processing one item");
                string f = "";
                foreach(string i in batchedItems.Keys)
                {
                    f = i;
                    break;
                }
                ActionPack itm = null;
                if (!string.IsNullOrEmpty(f) && batchedItems.TryRemove(f, out itm))
                {
                    switch(itm.type)
                    {
                        case ActionType.BuyFree:
                            Debug.Log($"Found item to run: {itm.item.sku}x{itm.item.quantity} {itm.item.name} ");
                            purchaseFreeItemSingle(itm.item, itm.Quantity, itm.OnSuccessOrderId, (Error err) => {
                                if (err.statusCode == "429")
                                {
                                    waitBetweenItems += 0.1f;
                                    Debug.LogError($"Increased wait to {waitBetweenItems}");
                                    purchaseFreeItem(itm.item, itm.OnSuccessOrderId, itm.OnError);
                                } else { OnError(err); itm.OnError.Invoke(err); }
                            });
                            break;
                        case ActionType.BuyVirtual:
                            if (itm.bundle != null)
                            {
                                purchaseBundleSingle(itm.bundle, itm.Quantity, itm.OnSuccessOrderId, (Error err) =>
                                {
                                    if (err.statusCode == "429")
                                    {
                                        waitBetweenItems += 0.1f;
                                        Debug.LogError($"Increased wait to {waitBetweenItems}");
                                        purchaseItem(itm.bundle, itm.Quantity, itm.OnSuccessOrderId, itm.OnError);
                                    }
                                    else { OnError(err); itm.OnError.Invoke(err); }
                                });
                            }
                            else if (itm.item != null)
                            {
                                purchaseItemSingle(itm.item, itm.Quantity, itm.OnSuccessOrderId, (Error err) =>
                                {
                                    if (err.statusCode == "429")
                                    {
                                        waitBetweenItems += 0.1f;
                                        Debug.LogError($"Increased wait to {waitBetweenItems}");
                                        purchaseItem(itm.item, itm.OnSuccessOrderId, itm.OnError);
                                    }
                                    else { OnError(err); itm.OnError.Invoke(err); }
                                });
                            } else if(itm.storeItem != null)
                            {

                            }
                            break;
                        case ActionType.Consume:
                            consumeItemSingle(itm.item, itm.Quantity, itm.OnSuccess, (Error err) => {
                                if(err.statusCode == "429")
                                {
                                    waitBetweenItems += 0.1f;
                                    Debug.LogError($"Increased wait to {waitBetweenItems}");
                                    consumeItem(itm.item, itm.OnSuccess, itm.OnError);
                                }
                                else { OnError(err); itm.OnError.Invoke(err); }
                            });
                            break;
                        default:
                            break;

                    }
                    yield return new WaitForSeconds(waitBetweenItems);
                }
            }
            waitBetweenItems = originalWait;
            yield return new WaitForSeconds(waitBetweenBatches);
        }
    }

    public void consumeItem(InventoryItem item, Action onSuccess, Action<Error> onError)
    {
        doItem(item, ActionType.Consume, (OrderId o) => { }, onSuccess, onError);
    }

    private void consumeItemSingle(InventoryItem item, int quantity, Action onSuccess, Action<Error> onError)
    {
        ConsumeItem s = new ConsumeItem()
        {
            sku = item.sku,
            quantity = (int?)quantity,
            instance_id = item.instance_id
        };
        XsollaInventory.ConsumeInventoryItem(s, onSuccess, onError);
    }

    private void purchaseFreeItemSingle(InventoryItem item, int quantity, Action<OrderId> onSuccess, Action<Error> onError)
    {
        GameController.AddLog("AuthManager", $"Purchasing free item {item.sku}");
        Debug.Log($"Buying {item.name}, up {quantity}. {item.description}");
        XsollaPurchaseFreeItem(item.sku, data =>
        {
            Debug.Log($"Updating {item.name} count, up {quantity}. {item.description}");
            GameController.AddLog("AuthManager", $"Updating {item.name} count, up {quantity}. {item.description}");
            lastWrite = Time.fixedTime;
            onAddSuccess?.Invoke(item);
            onSuccess?.Invoke(data);
        }, (Error err) => { onError(err); OnError(err); }, new PurchaseParams() { quantity = (int?)quantity });
    }
    private IEnumerator PauseRefresh(float delay, Action<InventoryItems> onSuccess)
    {
        while (pullingInventory || batchedItems.Count > 0) { yield return new WaitForEndOfFrame(); }
        pullingInventory = true;
        waitingInventory = true;
        float sinceLast = Time.fixedTime - lastWrite;
        if (sinceLast < minimumWaitAfterWrite && delay < minimumWaitAfterWrite - sinceLast) { delay = minimumWaitAfterWrite - sinceLast; }
        if (delay > 0f) { yield return new WaitForSeconds(delay); }
        GameController.AddLog("AuthManager", "Populating inventory");
        waitingInventory = false;
        XsollaInventory.GetInventoryItems((InventoryItems itms) => {
            OnInventorySuccess(itms);
            onSuccess?.Invoke(itms);
            pullingInventory = false;
        }, OnError, 5000, 0);
        yield return null;
    }
    private void OnError(Error error)
    {
        pullingInventory = false;
        Debug.LogError($"AUTHMANAGER: {error}");
        GameController.AddLog("AuthManager", $"Error: {error.errorMessage}");
        //UnityEngine.Debug.Log($"ERROR. Description: {error.errorMessage}");
        if (onError != null)
        {
            Debug.Log($"Invoking error: {error.errorMessage}");
            onError.Invoke(error.errorMessage);
        }
    }
    private void OnGetUserInfoSuccess(UserInfo userInfo, Action onSuccess)
    {
        //var levelEntryHead = info.email ?? info.username ?? info.nickname ?? info.last_name; info.picture
        //userObj.vcurrencyBalances = new List<VirtualCurrencyBalance>(balance.items);
        GameController.AddLog("AuthManager", "Got user info");
        userObj.id = userInfo.id;
        userObj.username = userInfo.username;
        userObj.nickname = userInfo.nickname;
        userObj.name = userInfo.name;
        userObj.picture = userInfo.picture;
        userObj.birthday = userInfo.birthday;
        userObj.first_name = userInfo.first_name;
        userObj.last_name = userInfo.last_name;
        userObj.gender = userInfo.gender;
        userObj.phone = userInfo.phone;
        userObj.groups = userInfo.groups;
        userObj.registered = userInfo.registered;
        userObj.external_id = userInfo.external_id;
        userObj.last_login = userInfo.last_login;
        userObj.ban = userInfo.ban;
        userObj.country = userInfo.country;
        userObj.tag = userInfo.tag;
        userObj.connection_information = userInfo.connection_information;
        userObj.is_anonymous = userInfo.is_anonymous;
        userObj.phone_auth = userInfo.phone_auth;
        userObj.presence = userInfo.presence;

        if(userInfo.devices != null && userInfo.devices.Count > 0)
        {
            foreach (UserDeviceInfo d in userInfo.devices) {
                XsollaUserAccount.UnlinkDeviceFromAccount(d.id, () => {
                    Debug.Log($"Unlinked from device: {d.id} {d.device}"); 
                    if(d == userInfo.devices[^1])
                    {
                        initiateSignIn(userObj.email, userObj.password, onSuccess, (Error e) => { });
                    }
                }, (Error ue) => { Debug.LogError($"Unable to unlink from device: {ue}"); });
            }
        } else
        {
            onSuccess?.Invoke();
            OnSignInSuccess();
        }

        Firebase.Analytics.FirebaseAnalytics.SetUserId(userInfo.email);
        Firebase.Crashlytics.Crashlytics.SetUserId(userInfo.email);
    }
    private void OnBalanceRequestSuccess(VirtualCurrencyBalances balance)
    {
        GameController.AddLog("AuthManager", "Got user vcurrency balances");
        userObj.vcurrencyBalances = new List<VirtualCurrencyBalance>(balance.items);
        balancesPopulated.Invoke(userObj);
    }
    private void OnInventorySuccess(InventoryItems inventory)
    {
        GameController.AddLog("AuthManager", "Got user inventory");
        if (inventory == null)
        {
            Debug.LogError("Inventory request successful but inventory is null");
            GameController.AddLog("AuthManager", "Inventory request successful but inventory is null");
        }
        userObj.vcurrencyBalances = new List<VirtualCurrencyBalance>();
        userObj.inventoryItems = new List<InventoryItem>();
        userObj.goalItems = new GoalItemManager();
        for (int i = 0; i < inventory.items.Length; i++)
        {
            InventoryItem item = inventory.items[i];
            //Debug.Log($"inventory loop {i}, sku {item.sku}");
            if (!string.IsNullOrEmpty(item.image_url))
            {
                GameController.Instance.imageLoader.GetImageAsync(item.image_url, (url, sprite) => {
                    if(onSpriteLoad != null)
                    {
                        onSpriteLoad.Invoke(item);
                    }
                });
            }
            if (item.VirtualItemType == VirtualItemType.VirtualCurrency)
            {
                userObj.vcurrencyBalances.Add(
                    new VirtualCurrencyBalance() {
                        sku = item.sku,
                        type = item.type,
                        name = item.name,
                        amount = (int)item.quantity,
                        description = item.description,
                        image_url = item.image_url,
                    }
                );
                //Debug.Log($"Added vcurrency {item.sku}");
            } else
            {
                if (item.attributes != null)
                {
                    for (int j = 0; j < item.attributes.Length; j++)
                    {
                        if (item.attributes[j].external_id == ATTRIBUTE_INVENTORY_SHOW && item.attributes[j].values[0].external_id == ATTRIBUTE_INVENTORY_SHOW_VALUE)
                        {
                            //Debug.Log($"item {item.sku} has INVENTORY_SHOW attribute. Adding.");
                            userObj.inventoryItems.Add(item);
                            //Debug.Log($"Added inventory item {item.sku}");
                        } else if (item.attributes[j].external_id.StartsWith(ATTRIBUTE_GOAL_ITEM) && item.attributes[j].external_id != ATTRIBUTE_GOAL_ITEM_ORDER)
                        {
                            int goalId;
                            if (Int32.TryParse(item.attributes[j].values[0].external_id.Substring(GOAL_ITEM_PREFIX.Length), out goalId))
                            {
                                userObj.goalItems.AddItem(goalId - 1, item);
                            } else
                            {
                                Debug.LogError($"{item.sku} has attr {ATTRIBUTE_GOAL_ITEM} but {item.attributes[j].values[0].external_id} is not in form {GOAL_ITEM_PREFIX}123.");
                            }
                        }
                    }
                } else
                {
                    //Debug.Log($"item {item.sku} is not a member of a group.");
                }
            }
        }
        balancesPopulated?.Invoke(userObj);
        inventoryPopulated?.Invoke(userObj);
    }

    private void SellItem(InventoryItem sellItem)
    {
        GameController.AddLog("AuthManager", $"Selling item {sellItem.sku}");
        if (sellItem.attributes.Length < 1)
        {
            throw new ItemNotSellableException("Item does not have currency value attribute.");
        }
        SellItemForCurrency(sellItem, RIS_CURRENCY_SKU);
    }
    public InventorySellDescriptor GetSellInfoByItem(InventoryItem it)
    {
        return GetSellInfoByItem(it, RIS_CURRENCY_SKU);
    }
    public InventorySellDescriptor GetSellInfoByItem(InventoryItem it, string vcurrencySku)
    {
        InventorySellDescriptor ret = new InventorySellDescriptor();
        ret.item = it;
        ret.currenySku = vcurrencySku;
        ret.currenctName = VCURENCY_SKU_TO_NAME[vcurrencySku];
        if (it.attributes == null || it.attributes.Length < 1)
        {
            return ret;
        }
        if (it.quantity < 1)
        {
            return ret;
        }
        int itemCurrencyValue = 0;
        for (int i = 0; i < it.attributes.Length; i++)
        {
            if (it.attributes[i].external_id == CURRENCY_VALUE_ATTRIBUTE_ID)
            {
                if (it.attributes[i].values.Length < 1)
                {
                    return ret;
                }
                for (int j = 0; j < it.attributes[i].values.Length; j++)
                {
                    //values are in the format "ris_#" where # is the numerical exchange value
                    if (it.attributes[i].values[j].external_id.Contains(RIS_CURRENCY_VALUE_PREFIX))
                    {
                        if (Int32.TryParse(it.attributes[i].values[j].external_id.Substring(RIS_CURRENCY_VALUE_PREFIX.Length), out itemCurrencyValue))
                        {
                            ret.isSellable = true;
                            ret.exchangeRate = itemCurrencyValue;
                            return ret;
                        }
                    }
                }

                if (itemCurrencyValue == 0)
                {
                    return ret;
                }
            }
        }
        return ret;
    }
    private void SellItemForCurrency(InventoryItem sellItem, string vcurrencySku)
    {
        GameController.AddLog("AuthManager", $"Selling item for currency {vcurrencySku}");
        if (sellItem.attributes.Length < 1)
        {
            throw new ItemNotSellableException("Item does not have currency value attribute.");
        }
        if (sellItem.quantity < 1)
        {
            throw new ItemNotSellableException("Item quantity must be greater than zero.");
        }
        int itemCurrencyValue = 0;
        bool foundCurrency = false;
        for (int i = 0; i < sellItem.attributes.Length && !foundCurrency; i++)
        {
            if (sellItem.attributes[i].external_id == CURRENCY_VALUE_ATTRIBUTE_ID)
            {
                if (sellItem.attributes[i].values.Length < 1)
                {
                    throw new ItemNotSellableException("Item does not have currency value attribute.");
                }
                bool foundValue = false;
                for (int j = 0; j < sellItem.attributes[i].values.Length && !foundValue; j++)
                {
                    //values are in the format "ris_#" where # is the numerical exchange value
                    if (sellItem.attributes[i].values[j].external_id.Contains(RIS_CURRENCY_VALUE_PREFIX))
                    {
                        if (Int32.TryParse(sellItem.attributes[i].values[j].external_id.Substring(RIS_CURRENCY_VALUE_PREFIX.Length), out itemCurrencyValue))
                        {
                            foundValue = true;
                        }
                    }
                }

                if (!foundValue || itemCurrencyValue == 0)
                {
                    throw new ItemNotSellableException("No currency quantity set for item value.");
                }
                foundCurrency = true;
            }
        }
        InventoryItem holder = new InventoryItem
        {
            sku = sellItem.sku,
            name = sellItem.name,
            quantity = sellItem.quantity,
            instance_id = sellItem.instance_id
        };
        Debug.Log($"Consuming item {sellItem.sku}, quantity {sellItem.quantity}.");
        GameController.AddLog("AuthManager", $"Consuming item {sellItem.sku}, quantity {sellItem.quantity}.");
        XsollaInventory.ConsumeInventoryItem(new ConsumeItem()
        {
            sku = sellItem.sku,
            quantity = (int?)sellItem.quantity,
            instance_id = sellItem.instance_id
        }, () =>
        {
            //item has been consumed, time to get currency for it
            lastWrite = Time.fixedTime;
            int? nQuantity = (int?)(itemCurrencyValue * holder.quantity);
            Debug.Log($"Item consumed. Purchasing currency package {VCURRENCY_PACKAGE_SKU}. {holder.sku} x {holder.quantity} * {itemCurrencyValue} = {nQuantity}");
            GameController.AddLog("AuthManager", $"Item consumed. Purchasing currency package {VCURRENCY_PACKAGE_SKU}. {holder.sku} x {holder.quantity} * {itemCurrencyValue} = {nQuantity}");

            XsollaPurchaseFreeItem(VCURRENCY_PACKAGE_SKU, data =>
            {
                Debug.Log($"Purchased currency package. Refreshing inventory.");
                GameController.AddLog("AuthManager", $"Purchased currency package");
                lastWrite = Time.fixedTime;
                onItemSold.Invoke(sellItem);
                risChange.Invoke((int)nQuantity);
                PopulateInventory(2f);
            }, (Error e) =>
            {
                Debug.LogError($"{e}, reverting to getting {holder}");
                GameController.AddLog("AuthManager", $"Cannot purchase currency. Error {e.errorMessage}. Reverting inventory transaction.");
                //encountered error purchasing currency, return items to inventory
                purchaseFreeItem(holder, (OrderId o) => { }, (Error e) => { });
            }, new PurchaseParams() { quantity = nQuantity });
        }, OnError);
        //XsollaInventory.Instance.GetInventoryItems(XsollaSettings.StoreProjectId, OnInventorySuccess, OnError);
    }

    public XsollaUser GetUserInfo
    {
        get
        {
            return userObj;
        }
    }
    private void XsollaPurchaseFreeItem(string itemSku, [CanBeNull] Action<OrderId> onSuccess, [CanBeNull] Action<Error> onError, PurchaseParams purchaseParams = null, Dictionary<string, string> customHeaders = null)
    {
        //var tempPurchaseParams = PurchaseParamsGenerator.GenerateTempPurchaseParams(purchaseParams);
        //var url = string.Format(URL_FREE_ITEM, projectId, itemSku);
        //var paymentHeaders = PurchaseParamsGenerator.GetPaymentHeaders(Token.Instance, customHeaders);
        //WebRequestHelper.Instance.PostRequest<PurchaseData, TempPurchaseParams>(SdkType.Store, url, tempPurchaseParams, paymentHeaders, onSuccess,
        //    onError: error => TokenRefresh.Instance.CheckInvalidToken(error, onError, () => XsollaPurchaseFreeItem(projectId, itemSku, onSuccess, onError, purchaseParams, customHeaders)),
        //    ErrorCheckType.BuyItemErrors);
        XsollaCatalog.CreateOrderWithFreeItem(itemSku, onSuccess, onError, purchaseParams, customHeaders);
    }
    private void OnDestroy()
    {
        if (batchedItems != null)
        {
            try
            {
                StopCoroutine(batchItems);
            } catch (NullReferenceException) { 
            } catch (Exception e)
            {
                GameController.LogException(e);
            }
        }
    }

    [Serializable]
    public class ItemNotSellableException : Exception
    {
        public ItemNotSellableException()
        {
        }

        public ItemNotSellableException(string message) : base(message)
        {
        }

        public ItemNotSellableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ItemNotSellableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}