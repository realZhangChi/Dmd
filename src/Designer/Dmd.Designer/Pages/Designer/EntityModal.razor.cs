using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Dmd.Designer.Components.Canvas;
using Dmd.Designer.Models;
using Dmd.Designer.Services;
using Microsoft.AspNetCore.Components;

namespace Dmd.Designer.Pages.Designer
{
    public partial class EntityModal
    {
        private Modal _modal;

        public ClassModel EntityModel { get; set; }

        [Inject]
        private IBrowserService BrowserService { get; set; }

        [Parameter]
        public EventCallback<ModalSaveClickEventArgs> ModalSaveClickCallBack { get; set; }

        public EntityModal()
        {
            EntityModel = new ClassModel();
        }

        private void CloseModal()
        {
            _modal.Hide();
        }

        public Task OpenAsync()
        {
            EntityModel = new ClassModel()
            {
                Properties = new List<string>() { string.Empty }
            };
            _modal.Show();

            return Task.CompletedTask;
        }

        private void AddProperty()
        {
            EntityModel.Properties.Add(string.Empty);
        }

        private async Task SaveAsync()
        {
            await ModalSaveClickCallBack.InvokeAsync(new ModalSaveClickEventArgs(EntityModel));
            _modal.Hide();
        }
    }
}
