using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitWings.Inventory
{

    [Serializable]
    public class SKUCount : IEquatable<SKUCount>
    {
        public static SKUCount operator ++(SKUCount c)
        {
            c.count++;
            return c;
        }
        public static SKUCount operator +(SKUCount c1, int c2)
        {
            c1.count += c2;
            return c1;
        }
        public static SKUCount operator +(SKUCount c1, SKUCount c2)
        {
            c1.count += c2.count;
            return c1;
        }
        public string sku;
        public int count;

        public bool Equals(SKUCount other)
        {
            return sku == other.sku;
        }
    }
}