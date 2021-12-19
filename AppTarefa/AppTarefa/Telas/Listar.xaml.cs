using AppTarefa.Banco;
using AppTarefa.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTarefa.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listar : ContentPage
    {
        public List<Tarefa> Lista { get; set; }
        public Listar()
        {
            InitializeComponent();

            Task.Run(() => {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Lista = await new TarefaDB().PesquisarAsync(DateTime.Now);
                    CVListaDeTarefas.ItemsSource = Lista;
                });
            });

            MessagingCenter.Subscribe<Listar, Tarefa>(this, "OnTarefaCadastrada", (sender, tarefa) => {
                if(Lista != null)
                {
                    Lista.Add(tarefa);
                }         
            });
        }

        private void BtnCadastrar(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Cadastrar());
;        }

        private void BtnVisualizar(object sender, EventArgs e)
        {
            var evento = (TappedEventArgs)e;
            var tarefa = (Tarefa)evento.Parameter;

            Navigation.PushAsync(new Visualizar(tarefa));
        }
    }
}