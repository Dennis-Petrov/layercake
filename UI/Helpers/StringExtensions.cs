using System;
using System.Security.Cryptography;
using System.Text;

namespace LayerCake.UI.Helpers
{
    /// <summary>
    /// Предоставляет методы-расширения для <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        #region Константы

        private const Int32 defaultFragmentLength = 15;

        #endregion

        #region Public-методы

        /// <summary>
        /// Зашифровывает строку.
        /// </summary>
        /// <param name="unSecuredString">
        /// Исходная строка.
        /// </param>
        /// <returns>
        /// Зашифрованная строка.
        /// </returns>
        public static String Encrypt(this String unSecuredString)
        {
            if (String.IsNullOrEmpty(unSecuredString))
                return unSecuredString;

            var decryptedData = Encoding.UTF8.GetBytes(unSecuredString);
            var encryptedData = ProtectedData.Protect(decryptedData, null, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Расшифровывает строку.
        /// </summary>
        /// <param name="securedString">
        /// Зашифрованная строка.
        /// </param>
        /// <returns>
        /// Расшифрованная строка.
        /// </returns>
        public static String Decrypt(this String securedString)
        {
            if (String.IsNullOrEmpty(securedString))
                return securedString;

            var encryptedData = Convert.FromBase64String(securedString);
            var decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decryptedData);
        }

        /// <summary>
        /// Возвращает левую часть строки, ограниченную длиной фрагмента.
        /// </summary>
        /// <param name="source">
        /// Исходная строка.
        /// </param>
        /// <param name="fragmentLength">
        /// Длина фрагмента.
        /// </param>
        /// <returns>
        /// Левая часть строки, ограниченная длиной фрагмента, либо строка целиком, если длина фрагмента больше,
        /// чем длина строки.
        /// </returns>
        public static String LeftStr(this String source, Int32 fragmentLength = defaultFragmentLength)
        {
            if (String.IsNullOrEmpty(source))
                return source;

            return source.Length > fragmentLength ? source.Substring(0, fragmentLength) : source;
        }

        #endregion
    }
}
