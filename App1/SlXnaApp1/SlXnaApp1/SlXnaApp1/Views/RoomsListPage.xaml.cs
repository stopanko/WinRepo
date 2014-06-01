using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;

namespace SlXnaApp1
{
    public partial class RoomsListPage : PhoneApplicationPage
    {
        public RoomsListPage()
        {
            InitializeComponent();
            WarpClient game = WarpClient.GetInstance();
            //game.AddConnectionRequestListener(new ConListen(this));
            //game.AddZoneRequestListener(new ZoneReqListener(this));
            //game.AddRoomRequestListener(new RoomReqListener(this));
            //game.AddNotificationListener(new NotificationListener(this));
            //game.AddLobbyRequestListener(new LobbyReqListen(this));
        }
    }
}