﻿// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Apis.Compute.v1.Data;
using GoogleCloudExtension.DataSources;
using System.ComponentModel;

namespace GoogleCloudExtension.CloudExplorerSources.Gce
{
    public class WindowsInstanceItem : GceInstanceItem
    {
        const string Category = "Windows Properties";

        private readonly WindowsInstanceInfo _info;

        public WindowsInstanceItem(Instance instance) : base(instance)
        {
            _info = instance.GetWindowsInstanceInfo();
        }

        [Category(Category)]
        [DisplayName("Windows Version")]
        [Description("The version of Windows installed on this instance.")]
        public string WindowsDisplayName => _info.DisplayName;
    }
}