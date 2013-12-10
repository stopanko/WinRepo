﻿using System;
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
using Microsoft.Xna.Framework;

namespace SlXnaApp1
{
    public class GameNotificationListener : com.shephertz.app42.gaming.multiplayer.client.listener.NotifyListener
    {
        private GamePage _game_page;
        //private GamePage game_page = new GamePage();

        public GameNotificationListener(GamePage page)
        {
            _game_page = page;
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
            //_game_page.showResult(username + " joined " + eventObj.getId());
        }

        public void onUserLeftLobby(LobbyData eventObj, String username)
        {
        }
        public void onUserJoinedLobby(LobbyData eventObj, String username)
        {
        }

        public void onChatReceived(ChatEvent eventObj)
        {
            //_game_page.showResult("chat from " + eventObj.getSender() + " msg " + eventObj.getMessage() + " id "+eventObj.getLocationId() + eventObj.isLocationLobby());
            //WarpClient.GetInstance().GetLiveLobbyInfo();
        }
        
        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {
            //string j = System.Text.UTF8Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length);
           // _page.showResult("update recvd " + j );

            JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length));
            int it = int.Parse(jsonObj["Item"].ToString());
            //string name = System.Text.Encoding.UTF8.GetString(eventObj.getUpdate(), 0, eventObj.getUpdate().Length);
            //_page.showResult(name);
            //_game_page.SendTxt = "Send";
            //GameTimerEventArgs e;
            //_game_page.Vect(eventObj);
            //_game_page.dir(e, new Vector2(3,3)); 
            GamePage.Balls_mas[it].GetDates(eventObj);


        }
        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<String, Object> properties)
        {
            //_game_page.showResult("Notification for User Changed Room Propert received");
            //_game_page.showResult(roomData.getId());
            //_game_page.showResult(sender);
            foreach (KeyValuePair<string, object> entry in properties)
            {
                //_game_page.showResult("KEY:" + entry.Key);
                //_game_page.showResult("VALUE:" + entry.Value.ToString());
            }
        }
    }
}
