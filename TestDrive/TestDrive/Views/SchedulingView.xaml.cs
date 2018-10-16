﻿using System;
using TestDrive.models;
using TestDrive.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestDrive.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulingView : ContentPage
    {
        public SchedulinViewModel ViewModel { get; set; }

        public SchedulingView(Vehicle vehicle)
        {
            InitializeComponent();
            ViewModel = new SchedulinViewModel(vehicle);
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Scheduling>(this, "Scheduling", async (message) =>
            {
                var confirm = await DisplayAlert("Save scheduling", "Do you want to send the scheduling?", "Yes", "No");

                if (confirm)
                {
                    this.ViewModel.SaveScheduling();
                }
            }
            );

            MessagingCenter.Subscribe<Scheduling>(this, "Successful Scheduling", (message) =>
            {
                DisplayAlert("Scheduling", string.Format(
                   @"Vehivle: {0}
                    Name: {1}
                    Phone: {2}
                    E-mail: {3}
                    Scheduling Date: {4}
                    Scheduling Time:{5}",
                   message.Vehicle.Name,
                   message.Name,
                   message.Telephone,
                   message.Email,
                   message.SchedulingDate.ToString("dd/MM/yyy"),
                   message.SchedulingTime), "OK");
            });
            
            MessagingCenter.Subscribe<ArgumentException>(this, "Fail Scheduling", (message) =>
            {
                DisplayAlert("Error", "It was not possible to schedule you test drive. Verify you information and attemp again later", "Ok");
            });
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Scheduling>(this, "Scheduling");
            MessagingCenter.Unsubscribe<Scheduling>(this, "Successful Scheduling");
            MessagingCenter.Unsubscribe<ArgumentException>(this, "Fail Scheduling");
        }
    }
}