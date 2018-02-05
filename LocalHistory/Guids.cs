// ReSharper disable InconsistentNaming

namespace LOSTALLOY.LocalHistory {
    using System;


    internal static class GuidList {

        #region Constants

        /// <summary>
        ///     Must match what's in the vsct file (or the menus won't work)
        /// </summary>
        public const string guidLocalHistoryPkgString = "48473bd6-841e-4368-8c7a-a5ea9fad7081";

        #endregion


        #region Static Fields

        /// <summary>
        ///     Must match what's in the vsct file (or the menus won't work)
        /// </summary>
        public static readonly Guid guidLocalHistoryCmdSet = new Guid("9c0eb15a-b3d9-4d2c-b4eb-57703d1ee539");

        #endregion

    };
}
