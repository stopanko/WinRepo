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

namespace SlXnaApp1
{
    public class LobbyReqListen : com.shephertz.app42.gaming.multiplayer.client.listener.LobbyRequestListener
    {
        private RoomPage _page;

        public LobbyReqListen(RoomPage page)
        {
            _page = page;
        }

        public void onGetLiveLobbyInfoDone(LobbyEvent eventObj)
        {
            WarpClient.GetInstance().SetCustomUserData("dc", "good lad");
        }
        public void onLeaveLobbyDone(LobbyEvent eventObj)
        {
        }
        public void onSubscribeLobbyDone(LobbyEvent eventObj)
        {
            //WarpClient.GetInstance().SendChat("yo yo");
        }
        public void onUnSubscribeLobbyDone(LobbyEvent eventObj)
        {
            WarpClient.GetInstance().SendChat("yo yo");
        }

        public void onGetLiveLobbyInfoDone(LiveRoomInfoEvent eventObj)
        {
            String[] users = eventObj.getJoinedUsers();
            if (users != null && users.Length > 0)
            {
                for (int i = 0; i < users.Length; i++ )
                    _page.showResult(users[i]);
            }
        }
    }
}
