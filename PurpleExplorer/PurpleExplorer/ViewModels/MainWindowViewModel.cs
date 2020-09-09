﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Azure.ServiceBus.Management;
using PurpleExplorer.Helpers;
using PurpleExplorer.Models;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.DTO;
using Avalonia;
using Avalonia.Controls;

namespace PurpleExplorer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ServiceBusResource> ConnectedServiceBuses { get; }
        public ObservableCollection<Message> Messages { get; }
        public string ConnectionString { get; set; }
        public MainWindowViewModel()
        {
            ConnectedServiceBuses =
                new ObservableCollection<ServiceBusResource>(new[]
                {
                    new ServiceBusResource
                    {
                        Name = "ServiceBus1",
                        Topics = new ObservableCollection<ServiceBusTopic>(new []
                        {
                            new ServiceBusTopic
                            {
                                Name = "Topic1-1",
                                Subscriptions = new ObservableCollection<ServiceBusSubscription>(new []
                                {
                                    new ServiceBusSubscription {Name = "Subscription-1"},
                                    new ServiceBusSubscription {Name = "Subscription-2"}
                                })
                            }
                        })
                    }
                });
            
            Messages = new ObservableCollection<Message>(GenerateMockMessages());
        }
        private IEnumerable<Message> GenerateMockMessages()
        {
            var mockMessages = new List<Message>()
            {
                new Message()
                {
                    Content = "Test Message 1",
                    Size = 1
                },
                new Message()
                {
                    Content = "Test Message 2",
                    Size = 2
                },
                new Message()
                {
                    Content = "Test Message 3",
                    Size = 3
                }
            };
            return mockMessages;
        }

        public async void BtnConnectCommand()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                try
                {
                    ServiceBusHelper helper = new ServiceBusHelper(ConnectionString);

                    var topics = await helper.GetTopics();

                    ConnectedServiceBuses[0].Topics.Clear();

                    foreach (var obj in topics)
                    {
                        ConnectedServiceBuses[0].Topics.Add(new ServiceBusTopic()
                        {
                            Name = obj.Name
                        });
                    }
                }
                catch (Exception ex)
                {
                    await MessageBoxHelper.ShowMessageBox(ButtonEnum.Ok, "Error", "The connection string is invalid.", Icon.Error);
                }
            }
            else
            {
                await MessageBoxHelper.ShowMessageBox(ButtonEnum.Ok, "Error", "The connection string is missing.", Icon.Error);
            }
        }
    }
}