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
using  System.Collections.Generic;
using System.Threading;

namespace SlXnaApp1
{
    public class ZoneReqListener : com.shephertz.app42.gaming.multiplayer.client.listener.ZoneRequestListener
    {
        private RoomsListPage _page;
        //public static List<Control> list = new List<Control>();
        //public static string[] Str;
        public ZoneReqListener(RoomsListPage page)
        {
            _page = page;
        }

        public void onDeleteRoomDone(RoomEvent eventObj)
        {
        }
        public void onGetAllRoomsDone(AllRoomsEvent eventObj)
        {
            int _length = eventObj.getRoomIds().Length;
            _page.showResult("rooms are " + _length);

            string[] Str = new string[_length];
            
            //public byte eventObj.getResult();  
            //public String[] s =  eventObj.getRoomIds(); 
            for (int j = 0; j < _length; j++)
            {
                Str[j] = eventObj.getRoomIds()[j];
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                //    HyperlinkButton b = new HyperlinkButton();
                //    b.Content = Str[j];
                //    b.Click += new RoutedEventHandler(_page.b_Click);
                //    _page.list.Add(b);
                //});
                //HyperlinkButton b = new HyperlinkButton();
                //b.Content = Str[j];
                //b.Click += new RoutedEventHandler(_page.b_Click);
                //RoomsListPage.list.Add(b);
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _page.createRoomsList(Str); //викликаємо процедуру створення списку кімеат
            });
            //for (int i = 0; i < _length; i++)
            //{
            //    //Str[i] = eventObj.getRoomIds()[i];
            //    Deployment.Current.Dispatcher.BeginInvoke(() =>
            //    {
            //            HyperlinkButton b = new HyperlinkButton();
            //            b.Content = Str[i];
            //            b.Click += new RoutedEventHandler(_page.b_Click);
            //            list.Add(b);
            //    });
            //    //event.getRoomIds()[i]
            //}

            //foreach (string s in Str)
            //{
            //    //WarpClient.GetInstance().GetLiveRoomInfo(s);
            //    Deployment.Current.Dispatcher.BeginInvoke(() =>
            //    {
            //        HyperlinkButton b = new HyperlinkButton();
            //        b.Content = s;
            //        b.Click += new RoutedEventHandler(_page.b_Click);
            //        list.Add(b);
            //    });


            //}

            //Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
            //    _page.listBox1.ItemsSource = list;
            //});

            
        }

        public void onCreateRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                WarpClient.GetInstance().SetCustomRoomData(eventObj.getData().getId(), "This is a sample room");
                _page.showResult("name " + eventObj.getData().getName() + " and Id " + eventObj.getData().getId());
            }
        }
        public void onGetOnlineUsersDone(AllUsersEvent eventObj)
        {
            _page.showResult("GetOnlineUser" + eventObj.getUserNames().Length);
        }
        public void onGetLiveUserInfoDone(LiveUserInfoEvent eventObj)
        {
            _page.showResult(eventObj.getCustomData() + " " + eventObj.isLocationLobby());
        }
        public void onSetCustomUserDataDone(LiveUserInfoEvent eventObj)
        {
            //WarpClient.GetInstance().GetLiveUserInfo("dc");
        }
        public void onGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                _page.showResult("GetMatchedRooms event received with success status");
                foreach (var roomData in eventObj.getRoomsData())
                {
                    _page.showResult("Room ID:" + roomData.getId());
                }
            }
        }

        //public void CreateRoom(String name, String owner, int maxUsers)  
        //{

        //}
    }
}
