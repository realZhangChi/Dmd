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

        public EntityModel EntityModel { get; set; }

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Parameter]
        public EventCallback<ModalSaveClickEventArgs> ModalSaveClickCallBack { get; set; }

        public EntityModal()
        {
            EntityModel = new EntityModel();
        }

        private void CloseModal()
        {
            _modal.Hide();
        }

        public Task OpenAsync()
        {
            EntityModel = new EntityModel()
            {
                Properties = new List<PropertyModel>() { new() }
            };
            _modal.Show();

            return Task.CompletedTask;
        }

        private async Task SaveAsync()
        {
            await ModalSaveClickCallBack.InvokeAsync(new ModalSaveClickEventArgs(EntityModel));
            _modal.Hide();
        }
    }
}
