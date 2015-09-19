// Copyright (c) Microsoft. All rights reserved.

using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Blinky
{
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 5;
		private const int MS_PIN = 11;
		
		
        private GpioPin pin;
		private GpioPin pin_ms;
        
		private GpioPinValue pinValue;
		private GpioPinValue pinValue_ms;
		
        
		private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
			if (pin != null)
            {
                timer.Start();
            } 
			Unloaded += MainPage_Unloaded;
            
			InitGPIO();
                  
        }
		private void MainPage_Unloaded(object sender, object args)
        {
            pin.Dispose();
            pin_ms.Dispose();
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            pin    = gpio.OpenPin(LED_PIN);
			pin_ms = gpio.OpenPin(MS_PIN);
			
			pin_ms.SetDriveMode(GpioPinDriveMode.Input);
			
           
            pin.Write(GpioPinValue.Low);
            
			pin.SetDriveMode(GpioPinDriveMode.Output);
			
			
            
			//pin.Write(pinValue);
            
			//pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioStatus.Text = "GPIO pin initialized correctly.";

        }

   



         
        private void FlipLED()
        {
            pinValue_ms = pin_ms.Read();
			
			if (pinValue_ms == GpioPinValue.High)
            {
                pin.Write(GpioPinValue.High);
            }
            else
            {
                pin.Write(GpioPinValue.Low);
            }
			
        }

        private void Timer_Tick(object sender, object e)
        {
            FlipLED();
        }
             

    }
}
