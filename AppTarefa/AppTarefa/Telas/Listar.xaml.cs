using AppTarefa.Banco;
using AppTarefa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTarefa.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listar : ContentPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Tarefa> _lista;
        public ObservableCollection<Tarefa> Lista
        {
            get
            {
                return _lista;
            }
            set
            {
                _lista = value;
                NotifyPropertyChanged("Lista");
            }
        }

        public Listar()
        {
            InitializeComponent();

            AtualizarDataCalendario(DateTime.Now);

            MessagingCenter.Subscribe<Listar, Tarefa>(this, "OnTarefaCadastrada", (sender, tarefa) =>
            {
                if (Lista != null)
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

        private async void BtnExcluir(object sender, EventArgs e)
        {
            bool pergunta = await DisplayAlert("Excluir", "Tem certeza que deseja excluir este registro?","Sim", "Não");

            if (pergunta)
            {
                var swipeItem = (SwipeItem)sender;
                var tarefa = (Tarefa)swipeItem.CommandParameter;

                var excluido = await new TarefaDB().ExcluirAsync(tarefa.Id);
                if (excluido)
                {
                    Lista.Remove(tarefa);
                }
            }
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var label = checkBox.Parent.FindByName<Label>("LblTarefaDetalhe");
            if(label != null)
            {
                var tapGesture = (TapGestureRecognizer)label.GestureRecognizers[0];
                if(tapGesture != null)
                {
                    var tarefa = (Tarefa)tapGesture.CommandParameter;
                    if (tarefa != null)
                    {
                        tarefa.Finalizada = e.Value;

                        await new TarefaDB().AtualizarAsync(tarefa);
                    }
                }
             
            }
        }

        private void AbrirCalendario(object sender, EventArgs e)
        {
            DPCalendario.Focus();
        }

        private void DateSelectedAction(object sender, DateChangedEventArgs e)
        {
            AtualizarDataCalendario(e.NewDate);
        }

        private void AtualizarDataCalendario(DateTime data)
        {
            Task.Run(() => {
                Device.BeginInvokeOnMainThread(async () => {
                    Lista = new ObservableCollection<Tarefa>(
                        await new TarefaDB().PesquisarAsync(data)
                    );
                    CVListaDeTarefas.ItemsSource = Lista;
                    QuatidadeTarefas.Text = Lista.Count.ToString();
                });
            });

            Dia.Text = data.Day.ToString();
            Mes.Text = data.Month.ToString();
            DiaDaSemana.Text = data.DayOfWeek.ToString();
        }
    }
}