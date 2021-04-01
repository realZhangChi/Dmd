using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Dmd.SourceOptions;
using Microsoft.AspNetCore.Components;

namespace Dmd.Designer.Pages.Designer
{
    public partial class EntityModal
    {
        private Modal _modal;
        
        [Parameter]
        public EntityModel EntityModel { get; set; }

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Parameter]
        public EventCallback<ModalSaveClickEventArgs> ModalSaveClickCallBack { get; set; }
        
        private void CloseModal()
        {
            _modal.Hide();
        }

        public Task OpenAsync()
        {
            _modal.Show();

            return Task.CompletedTask;
        }

        private async Task SaveAsync()
        {
            await ModalSaveClickCallBack.InvokeAsync();
            _modal.Hide();
        }
    }
}
