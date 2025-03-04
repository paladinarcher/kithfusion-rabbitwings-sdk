using RabbitWings.Catalog;
using RabbitWings.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{
    public static class Utils
    {
        private static string baseDirectory;

        public static string Url
        {
            get
            {
               return UtilsBase.Url;
            }
        }
        public static string ApiKey
        {
            get
            {
                return UtilsBase.ApiKey;
            }
        }
        public static string BaseDirectory
        {
            get
            {
                if (baseDirectory == null)
                {
                    baseDirectory = "transaction";
                }
                return baseDirectory;
            }
        }
        public static string BaseUserItemsDirectory
        {
            get
            {
                return "user/items";
            }
        }
        public static string BaseUserTotalsDirectory
        {
            get
            {
                return $"{BaseDirectory}/counts";
            }
        }

        public static void Consume(InventoryItemCount item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            InventoryItem i = new InventoryItem() { sku = item.sku, quantity = item.quantity };
            Consume(new List<InventoryItem> { i }, onComplete, onError);
        }
        public static void Consume(InventoryItem item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Consume(new List<InventoryItem> { item }, onComplete, onError);
        }

        public static void Consume(string sku, int quantity, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            InventoryItem i = new InventoryItem() { sku = sku, quantity = quantity };
            Consume(new List<InventoryItem> { i }, onComplete, onError);
        }

        public static void Consume(Dictionary<string, int> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItem> list = new List<InventoryItem>(items.Count);
            foreach (KeyValuePair<string, int> item in items)
            {
                InventoryItem i = new InventoryItem()
                {
                    sku = item.Key,
                    quantity = item.Value,
                };
                list.Add(i);
            }
            Consume(list, onComplete, onError);
        }

        public static void Consume(List<InventoryItemCount> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItem> list = new List<InventoryItem>(items.Count);
            foreach (InventoryItemCount item in items)
            {
                InventoryItem i = new InventoryItem()
                {
                    sku = item.sku,
                    quantity = item.quantity
                };
                list.Add(i);
            }
            Consume(list, onComplete, onError);
        }
        public static void Consume(List<InventoryItem> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            if (User.Current == null || User.Current.id == null)
            {
                onError?.Invoke(new Error(ErrorType.UserNotFound, "404", "404", "No current user logged in yet."));
                return;
            }
            bool foundValid = false;
            string errLines = "";
            List<OrderItem> orders = new List<OrderItem>(items.Count);
            foreach (InventoryItem item in items)
            {
                InventoryItem i = ItemMapCache.ItemHolder.GetItemBySku(item.sku);
                if (i == null)
                {
                    XDebug.LogWarning($"Item with sku {item.sku} was not found.");
                    errLines += (string.IsNullOrEmpty(errLines) ? "" : "\n") + $"Item with sku {item.sku} was not found.";
                    continue;
                }
                i.quantity = 0;
                item.MergeFrom(i);
                foundValid = true;
                if (item.quantity < 0)
                {
                    item.quantity = Mathf.Abs(item.quantity);
                }
                OrderItem oi = new()
                {
                    sku = item.sku,
                    quantity = 0 - item.quantity,
                    is_free = "yes",
                    price = null
                };
                orders.Add(oi);
            }
            if (!foundValid)
            {
                onError?.Invoke(new Error(ErrorType.Undefined, "", "", errLines));
                return;
            }

            OrderContent oc = new()
            {
                price = null,
                is_free = "yes",
                virtual_price = null,
                items = orders.ToArray()
            };

            DoTransaction(oc, onComplete, onError);
        }

        public static void Sell(InventoryItemCount item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            InventoryItem i = new InventoryItem() { sku = item.sku, quantity = item.quantity };
            Sell(new List<InventoryItem> { i }, onComplete, onError);
        }
        public static void Sell(InventoryItem item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Sell(new List<InventoryItem> { item }, onComplete, onError);
        }

        public static void Sell(string sku, int quantity, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            InventoryItem i = new InventoryItem() { sku = sku, quantity = quantity };
            Sell(new List<InventoryItem> { i }, onComplete, onError);
        }

        public static void Sell(Dictionary<string, int> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItem> list = new List<InventoryItem>(items.Count);
            foreach (KeyValuePair<string, int> item in items)
            {
                InventoryItem i = new InventoryItem()
                {
                    sku = item.Key,
                    quantity = item.Value,
                };
                list.Add(i);
            }
            Sell(list, onComplete, onError);
        }

        public static void Sell(List<InventoryItemCount> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItem> list = new List<InventoryItem>(items.Count);
            foreach (InventoryItemCount item in items)
            {
                InventoryItem i = new InventoryItem()
                {
                    sku = item.sku,
                    quantity = item.quantity
                };
                list.Add(i);
            }
            Sell(list, onComplete, onError);
        }
        public static void Sell(List<InventoryItem> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            if (User.Current == null || User.Current.id == null)
            {
                onError?.Invoke(new Error(ErrorType.UserNotFound, "404", "404", "No current user logged in yet."));
                return;
            }
            bool foundValid = false;
            string errLines = "";
            List<OrderItem> orders = new List<OrderItem>(items.Count);
            foreach (InventoryItem item in items) {
                InventoryItem i = ItemMapCache.ItemHolder.GetItemBySku(item.sku);
                if (i == null)
                {
                    XDebug.LogWarning($"Item with sku {item.sku} was not found.");
                    errLines += (string.IsNullOrEmpty(errLines) ? "" : "\n") + $"Item with sku {item.sku} was not found.";
                    continue;
                }
                i.quantity = 0;
                item.MergeFrom(i);
                InventorySellDescriptor sd = item.SellDescriptor();
                if (sd == null || !sd.isSellable)
                {
                    XDebug.LogWarning($"{item.name} ({item.sku}) cannot be sold, omitting.");
                    errLines += (string.IsNullOrEmpty(errLines) ? "" : "\n") + $"{item.name} ({item.sku}) cannot be sold, omitting.";
                    continue;
                }
                foundValid = true;
                if (item.quantity < 0) {
                    item.quantity = Mathf.Abs(item.quantity);
                }
                OrderItem oi = new()
                {
                    sku = item.sku,
                    quantity = 0 - item.quantity,
                    is_free = "no",
                    price = sd.Price
                };
                orders.Add(oi);
            }
            if(!foundValid)
            {
                onError?.Invoke(new Error(ErrorType.Undefined, "", "", errLines));
                return;
            }

            OrderContent oc = new()
            {
                price = null,
                is_free = "no",
                virtual_price = null,
                items = orders.ToArray()
            };

            DoTransaction(oc, onComplete, onError);
        }

        public static void Buy(string sku, int quantity, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            PurchasableItem bi = ItemMapCache.ItemHolder.GetBundleBySku(sku);
            if (bi != null)
            {
                Buy(bi, onComplete, onError);
                return;
            }
            PurchasableItem st = ItemMapCache.ItemHolder.GetStoreItemBySku(sku);
            if (st != null)
            {
                Buy(st, onComplete, onError);
                return;
            }

            onError?.Invoke(new Error(ErrorType.Undefined, "", "", $"No bundle with the sku {sku} was found."));
            return;
        }
        public static void Buy(Dictionary<string, int> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            bool foundValid = false;
            string errLines = "";
            List<PurchasableItem> list = new List<PurchasableItem>(items.Count);
            foreach (KeyValuePair<string, int> item in items)
            {
                PurchasableItem b = ItemMapCache.ItemHolder.GetBundleBySku(item.Key);
                if (b == null)
                {
                    b = ItemMapCache.ItemHolder.GetStoreItemBySku(item.Key);
                    if(b == null)
                    {
                        XDebug.LogWarning($"Item with SKU {item.Key} not found, omitting.");
                        errLines += (string.IsNullOrEmpty(errLines) ? "" : "\n") + $"Item with SKU {item.Key} not found, omitting.";
                        continue;
                    }
                }
                foundValid = true;
                list.Add(b);
            }
            if (!foundValid) {
                onError?.Invoke(new Error(ErrorType.Undefined, "", "", errLines));
                return;
            }
            Buy(list, onComplete, onError);
        }
        public static void Buy(PurchasableItem item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Buy(new List<PurchasableItem> { item }, onComplete, onError);
        }
        public static void Buy(List<PurchasableItem> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            bool foundValid = false;
            string errLines = "";

            List<OrderItem> orders = new List<OrderItem>(items.Count);

            foreach (PurchasableItem item in items)
            {
                VirtualPrice vp = null;
                foreach (VirtualPrice v in item.virtual_prices)
                {
                    if (v.sku == Constants.RIS_CURRENCY_SKU)
                    {
                        vp = v;
                        break;
                    }
                }
                if (vp == null)
                {
                    XDebug.LogWarning($"Bundle {item.name} ({item.sku}) cannot be bought, omitting.");
                    errLines += (string.IsNullOrEmpty(errLines) ? "" : "\n") + $"Bundle {item.name} ({item.sku}) cannot be bought, omitting.";
                    continue;
                }
                foundValid = true;
                OrderItem oi = new()
                {
                    sku = item.sku,
                    quantity = 1,
                    is_free = "no",
                    price = new Price()
                    {
                        amount = vp.amount,
                        amount_without_discount = vp.amount_without_discount,
                        currency = vp.sku
                    }
                };
                orders.Add(oi);
            }
            if (!foundValid)
            {
                onError?.Invoke(new Error(ErrorType.Undefined, "", "", errLines));
                return;
            }

            OrderContent oc = new()
            {
                price = null,
                is_free = "yes",
                virtual_price = null,
                items = orders.ToArray()
            };

            DoTransaction(oc, onComplete, onError);
        }

        public static void Give(InventoryItemCount item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Give(item.sku, item.quantity, onComplete, onError);
        }
        public static void Give(InventoryItem item, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Give(item.sku, item.quantity, onComplete, onError);
        }

        public static void Give(string sku, int quantity, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            Give(new List<InventoryItemCount> { new InventoryItemCount { quantity = quantity, sku = sku } }, onComplete, onError);
        }

        public static void Give(Dictionary<string, int> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItemCount> list = new List<InventoryItemCount>(items.Count);
            foreach(KeyValuePair<string, int> item in items)
            {
                list.Add(new InventoryItemCount { quantity = item.Value, sku = item.Key });
            }
            Give(list, onComplete, onError);
        }
        public static void Give(List<InventoryItem> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            List<InventoryItemCount> list = new List<InventoryItemCount>(items.Count);
            foreach (InventoryItem item in items)
            {
                list.Add(new InventoryItemCount { quantity = item.quantity, sku = item.sku });
            }
            Give(list, onComplete, onError);
        }

        public static void Give(List<InventoryItemCount> items, Action<TransactionResponse> onComplete, Action<Error> onError)
        {
            if (User.Current == null || User.Current.id == null)
            {
                onError?.Invoke(new Error(ErrorType.UserNotFound, "404", "404", "No current user logged in yet."));
                return;
            }
            List<OrderItem> orders = new List<OrderItem>(items.Count);
            foreach (InventoryItemCount item in items)
            {
                OrderItem oi = new()
                {
                    sku = item.sku,
                    quantity = item.quantity,
                    is_free = "yes",
                    price = null
                };
                orders.Add(oi);
            }

            OrderContent oc = new()
            {
                price = null,
                is_free = "yes",
                virtual_price = null,
                items = orders.ToArray()
            };

            DoTransaction(oc, onComplete, onError);
        }
        public static void RefreshTotals(Action<TransactionResponse> onSuccess, Action<Error> onError)
        {
            string getUrl = new UrlBuilder($"{Url}/{BaseUserTotalsDirectory}").Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };

            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (TransactionResponse r) => {
                    User.UpdateTotalCounts(r.totalCounts, true);
                    onSuccess?.Invoke(r);
                },
                onError,
                ErrorGroup.CommonErrors);
        }
        public static void RefreshItems(Action<TransactionResponse> onSuccess, Action<Error> onError)
        {
            string getUrl = new UrlBuilder($"{Url}/{BaseUserItemsDirectory}").Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };

            WebRequestHelper.Instance.GetRequest(
                SdkType.Login,
                getUrl,
                headers,
                (TransactionResponse r) => {
                    User.Current.UpdateItems(r.inventory, true);
                    onSuccess?.Invoke(r);
                },
                onError,
                ErrorGroup.CommonErrors);
        }
        public static void DoTransaction(OrderContent oc, Action<TransactionResponse> onComplete, Action<Error> onError) {

            string postUrl = new UrlBuilder($"{Url}/{BaseDirectory}")
                .Build();

            List<WebRequestHeader> headers = new List<WebRequestHeader>()
            {
                WebRequestHeader.AuthXApi(ApiKey),
                WebRequestHeader.JsonContentTypeHeader(),
                WebRequestHeader.CurrentUser()
            };


            WebRequestHelper.Instance.PostRequest(
                SdkType.Login,
                postUrl,
                oc,
                headers,
                (TransactionResponse r) => {
                    User.UpdateTotalCounts(r.totalCounts);
                    User.Current.UpdateItems(r.inventory);
                    User.Current.UpdateCurrency(r.vcurrencyBalance);
                    onComplete?.Invoke(r);
                },
                onError,
                ErrorGroup.CommonErrors);
        }


    }
}