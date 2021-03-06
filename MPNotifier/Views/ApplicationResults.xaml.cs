﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JobOffersProvider.Core;
using MPNotifier.Helpers;
using MPNotifier.Models;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class ApplicationResults : Page {
        private static Timer timer;

        private ApplicationSettingsModel settings { get; set; }

        public ApplicationResultsViewModel ViewModel { get; set; }

        private void PrepareApplicationData() {
            try {
                IoC.Resolve<IApplicationService>().PrepareApplicationData(this.settings);
            } catch (Exception) {
                //TODO: logger
            }
        }

        private void InitializeApplication() {
            this.PrepareApplicationData();
            
            this.InitializeControls();
            this.InitializeComponent();
        }

        private void ShowNotifications() {
            //this.ShowToastNotifications();
            this.StartApplicationsLoop();
        }

        private void ShowToastNotifications() {
            IoC.Resolve<INotificationsLoader>().ShowToastNotification(5);
        }

        private void StartApplicationsLoop() {
            timer = new Timer(x => this.ShowNotifications(), null, 1000 * 60 * 30, Timeout.Infinite);
        }

        private void InitializeControls() {
            var allOffers = IoC.Resolve<IApplicationResultsService>().GetAllJobOfferViewModels();
            this.ViewModel.Offers = new ObservableCollection<JobOfferViewModel>(allOffers);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var navigationModel = e.Parameter as NavigationModel;
            this.ViewModel = new ApplicationResultsViewModel();

            this.settings = (ApplicationSettingsModel) navigationModel?.Parameter;

            if (this.settings != null) {
                this.InitializeApplication();
                this.ShowNotifications();
            } else {
                this.InitializeControls();
                this.InitializeComponent();
            }
        }

        private void ResultsListView_OnItemClick(object sender, ItemClickEventArgs e) {
            var clickedItem = (JobOfferViewModel) e.ClickedItem;
            this.ShowOfferDetails(clickedItem.Id);
        }

        private void ShowOfferDetails(Guid clickedItemId) {
           NavigationHelper.Navigate(new NavigationModel{ViewType = typeof(OfferDetails), Parameter = clickedItemId});
        }
    }
}