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
        private const int LED_PIN = 27;


        private const int PB_PIN = 5;

        private const int LS_PIN = 26;


        private GpioPin pin;
        private GpioPin pushButton;
        private GpioPin lsButton;

        private DispatcherTimer timer;


        private GpioPinValue pushButtonValue;
        private GpioPinValue lsButtonValue;

        public MainPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            Unloaded += MainPage_Unloaded;

            InitGPIO();
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                pin = null;
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }
            pushButton = gpio.OpenPin(PB_PIN);
            pin = gpio.OpenPin(LED_PIN);
            lsButton = gpio.OpenPin(LS_PIN);

            pushButton.SetDriveMode(GpioPinDriveMode.Input);
            lsButton.SetDriveMode(GpioPinDriveMode.Input);

            pin.Write(GpioPinValue.Low);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioStatus.Text = "GPIO pin initialized correctly.";
        }

        private void MainPage_Unloaded(object sender, object args)
        {
            //pin.Dispose();
            //pushButton.Dispose();
        }

        private void FlipLED()
        {
            pushButtonValue = pushButton.Read();
            lsButtonValue = lsButton.Read();

            if (pushButtonValue == GpioPinValue.High && lsButtonValue == GpioPinValue.High)
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
