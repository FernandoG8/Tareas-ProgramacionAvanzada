using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private List<string> listaPalabras = new List<string>() { "Aurora", "Laberinto", "Serpiente", "Comida", "Refugio", "Horizonte", "Misterio", "Elefante", "Programacion", "JAVA", "Python", "Cristal", "Moto", "Carro", "Camioneta" };
        private List<char> letrasJugador = new List<char>();
        private List<char> letras = new List<char>();
        private List<char> escogidas = new List<char>();
        private string spotlight = "";
        private string palabraActual;
        private string mensaje = "¡Bienvenido al juego del ahorcado!";
        private string estado;
        private string imagen;
        private int errores;

        public MainPage()
        {
            InitializeComponent();

            Letras.AddRange("qwertyuiopasdfghjklzxcvbnm".ToCharArray());
            NuevaPalabra();
            ActualizarEstado();
            BindingContext = this;
        }

     
        public string Imagen
        {
            get => imagen;
            set
            {
                    imagen = value;
                    OnPropertyChanged();


            }
        }

        public string Estado
        {
            get => estado;
            set
            {
              
                    estado = value;
                    OnPropertyChanged();
            }
        }

        public string Spotlight
        {
            get => spotlight;
            set
            {
                    spotlight = value;
                    OnPropertyChanged();               
            }
        }

        public string Mensaje
        {
            get => mensaje;
            set
            {
                    mensaje = value;
                    OnPropertyChanged();   
            }
        }

        public List<char> Letras
        {
            get => letras;
            set
            {
                if (!Equals(letras, value))
                {
                    letras = value;
                    OnPropertyChanged();
                }
            }
        }
        private void OnLetterClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                char letra = button.Text[0];
                ManejarLetra(letra);
                button.IsEnabled = false;
            }
        }
        private void btnReiniciar(object sender, EventArgs e )
        {
            if(sender is Button boton)
            {
                NuevaPalabra();
                SwitchBotones(true);
                ActualizarEstado();
                Mensaje = "¡Bienvenido al juego del ahorcado!";
            }
        }

        private void EnmascararPalabra(string palabra, List<char> letras)
        {
            var mascara = palabra
                .Select(x => letras.Contains(char.ToLower(x)) ? x : '_')
                .ToArray();
            Spotlight = string.Join(" ", mascara);
        }

        public void NuevaPalabra()
        {
            Random rnd = new Random();
            int index = rnd.Next(listaPalabras.Count);
            palabraActual = listaPalabras[index].ToLower();
            letrasJugador.Clear();
            escogidas.Clear();
            errores = 0;
            EnmascararPalabra(palabraActual, letrasJugador);
        }

        private void ManejarLetra(char letra)
        {
            if (!escogidas.Contains(letra))
            {
                escogidas.Add(letra);

                if (palabraActual.Contains(letra))
                {
                    EnmascararPalabra(palabraActual, escogidas);
                    VerificarGanador();
                }
                else
                {
                    errores++;
                   
                }
            }
            ActualizarEstado();

        }

        private void VerificarGanador()
        {
            if (!Spotlight.Contains('_'))
            {
                Mensaje = "¡Felicidades, ganaste!";
                Estado = "Ganado";
                SwitchBotones(false);
            }
        }

        private void ActualizarEstado()
        {
            
            Estado = $"Errores: {errores} de 6";
            Imagen = $"img{errores}.jpg";

            //Mensaje para decirle que perdio 
            if (errores == 6)
            {
                Mensaje = "¡Has perdido! Presiona reiniciar para comenzar el juego de nuevo";
                Estado = "Perdiste nub";  
                Imagen = $"img{errores}.jpg";
                SwitchBotones(false);
            }

        }
        private void SwitchBotones(Boolean estado)
        { 
            var contenedor = this.FindByName <FlexLayout>("ContenedorBotones");
            
            foreach (var child in contenedor.Children) 
            {
                if (child is Button button)
                {
                    button.IsEnabled = estado;
                }
            }
        }
        
    }

}

