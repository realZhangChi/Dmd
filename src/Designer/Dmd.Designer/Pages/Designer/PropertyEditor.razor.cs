﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dmd.Designer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Dmd.Designer.Pages.Designer
{
    public partial class PropertyEditor
    {
        [CascadingParameter]
        public ClassModel EntityModel { get; set; }

        private void AddProperty()
        {
            EntityModel.Properties.Add(string.Empty);
        }
    }
}
