/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FinalYear.BoxMatch {

    public static class ExtentionMethods { // used for shuffeling generic list

        public static void ShuffleList<T>(this IList<T> list) {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider(); // using .Cryptography to increase inconsistency
            int n = list.Count;
            while (n > 1) {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
