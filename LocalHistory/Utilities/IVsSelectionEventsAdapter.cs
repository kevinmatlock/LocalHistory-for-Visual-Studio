// Copyright 2017 LOSTALLOY
// Copyright 2013 Intel Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// ReSharper disable IdentifierTypo
namespace LOSTALLOY.LocalHistory.Utilities {
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;


    // ReSharper disable once UnusedMember.Global
    internal class IVsSelectionEventsAdapter: IVsSelectionEvents {

        #region Public Methods and Operators

        public virtual int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive) {
            return VSConstants.S_OK;
        }


        public virtual int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew) {
            return VSConstants.S_OK;
        }


        public virtual int OnSelectionChanged(
            IVsHierarchy pHierOld,
            uint itemidOld,
            IVsMultiItemSelect pMISOld,
            ISelectionContainer pSCOld,
            IVsHierarchy pHierNew,
            uint itemidNew,
            IVsMultiItemSelect pMISNew,
            ISelectionContainer pSCNew) {
            return VSConstants.S_OK;
        }

        #endregion

    }
}
