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

namespace LOSTALLOY.LocalHistory.Utilities {
    using System;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;


    // ReSharper disable once UnusedMember.Global
    internal class IVsSolutionEventsAdapter: IVsSolutionEvents {

        #region Public Methods and Operators

        public virtual int OnAfterOpenSolution(object pUnkReserved, int fNewSolution) {
            return VSConstants.S_OK;
        }


        public virtual int OnAfterCloseSolution(
            object pUnkReserved) {
            return VSConstants.S_OK;
        }


        public virtual int OnAfterLoadProject(
            IVsHierarchy pStubHierarchy,
            IVsHierarchy pRealHierarchy) {
            return VSConstants.S_OK;
        }


        public virtual int OnAfterOpenProject(
            IVsHierarchy pHierarchy,
            int fAdded) {
            return VSConstants.S_OK;
        }


        public virtual int OnBeforeCloseProject(
            IVsHierarchy pHierarchy,
            int fRemoved) {
            return VSConstants.S_OK;
        }


        public virtual int OnBeforeCloseSolution(
            object pUnkReserved) {
            return VSConstants.S_OK;
        }


        public virtual int OnBeforeUnloadProject(
            IVsHierarchy pRealHierarchy,
            IVsHierarchy pStubHierarchy) {
            return VSConstants.S_OK;
        }


        public virtual int OnQueryCloseProject(
            IVsHierarchy pHierarchy,
            int fRemoving,

            // ReSharper disable once RedundantAssignment
            ref int pfCancel) {
            pfCancel = VSConstants.S_OK;

            return VSConstants.S_OK;
        }


        public virtual int OnQueryCloseSolution(
            object pUnkReserved,

            // ReSharper disable once RedundantAssignment
            ref int pfCancel) {
            pfCancel = VSConstants.S_OK;

            return VSConstants.S_OK;
        }


        public virtual int OnQueryUnloadProject(
            IVsHierarchy pRealHierarchy,

            // ReSharper disable once RedundantAssignment
            ref int pfCancel) {
            pfCancel = VSConstants.S_OK;

            return VSConstants.S_OK;
        }

        #endregion

    }
}
