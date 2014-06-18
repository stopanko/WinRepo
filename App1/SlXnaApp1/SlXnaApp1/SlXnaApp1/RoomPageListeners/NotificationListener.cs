using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SlXnaApp1
{
    public class NotificationListener : com.shephertz.app42.gaming.multiplayer.client.listener.NotifyListener
    {
        private RoomPage _page;
        //private GamePage game_page = new GamePage();

        public NotificationListener(RoomPage page)
        {
            _page = page;
        }

        public void onRoomCreated(RoomData eventObj)
        {
        }
        public void onRoomDestroyed(RoomData eventObj)
        {
        }
        public void onUserLeftRoom(RoomData eventObj, String username)
        {
        }
        public void onUserJoinedRoom(RoomData eventObj, String username)
        {
            _page.showResult(username + " joined " + eventObj.getId());
        }

        public void onUserLeftLobby(LobbyData eventObj, String username)
        {
        }
        public void onUserJoinedLobby(LobbyData eventObj, String username)
        {
        }

        public void onChatReceived(ChatEvent eventObj)
        {
            _page.showResult("chat from " + eventObj.getSender() + " msg " + eventObj.getMessage() + " id "+eventObj.getLocationId() + eventObj.isLocationLobby());
            string str = "";
            if (eventObj.getSender() == UserDates._UserName)
            {
                str = "";
            }
            
            else 
            {
                str = "\t\t\t\t\t\t\t";
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _page.chatList.Items.Add(str + eventObj.getMessage());
            });
            
            //List<Control> list = new List<Control>();
            //foreach (string s in str)
            //{                
            //    HyperlinkButton b = new HyperlinkButton();
            //    b.Content = s;
            //    b.Click += new RoutedEventHandler(b_Click);
            //    list.Add(b);
            //}
            //listBox1.ItemsSource = list;

            WarpClient.GetInstance().GetLiveLobbyInfo();
        }
        
        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {
            //string j = System.Text.UTF8Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length);
           // _page.showResult("update recvd " + j );

            //JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length));
            //string name = System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length);
            //_page.showResult(name);
            //game_page.SendTxt = "Send";
            //game_page.Vect(eventObj);



        }
        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<String, Object> properties)
        {
            _page.showResult("Notification for User Changed Room Propert received");
            _page.showResult(roomData.getId());
            _page.showResult(sender);
            foreach (KeyValuePair<string, object> entry in properties)
            {
                _page.showResult("KEY:" + entry.Key);
                _page.showResult("VALUE:" + entry.Value.ToString());
            }
        }
    }
}
