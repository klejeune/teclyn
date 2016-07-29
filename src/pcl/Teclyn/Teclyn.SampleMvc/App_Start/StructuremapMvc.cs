// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using StructureMap;
using Teclyn.Core;
using Teclyn.Core.Events;
using Teclyn.SampleCore.TodoLists.Events;
using Teclyn.SampleCore.TodoLists.Models;
using Teclyn.SampleMvc.App_Start;

using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace Teclyn.SampleMvc.App_Start {
	using System.Web.Mvc;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

	using Teclyn.SampleMvc.DependencyResolution;

    using StructureMap;
    
	public static class StructuremapMvc {
        #region Public Properties

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }
        public static IContainer Container { get; private set; }

        #endregion
		
		#region Public Methods and Operators
		
		public static void End() {
            StructureMapDependencyScope.Dispose();
        }
		
        public static void Start() {
            Container = new Container();

            Container.Configure(_ =>
            {
                _.Scan(x => x.Assembly(typeof(ITodoList).Assembly));
            });
            

            
            StructureMapDependencyScope = new StructureMapDependencyScope(Container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }

        #endregion
    }
}