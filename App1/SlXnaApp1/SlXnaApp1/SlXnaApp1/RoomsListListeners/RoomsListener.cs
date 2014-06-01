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
    public class RoomsListener : com.shephertz.app42.gaming.multiplayer.client.listener.RoomRequestListener
    {
        private RoomsListPage _page;
        public static int Maxusers;
        public RoomsListener(RoomsListPage page)
        {
            _page = page;
        }

        public void onSubscribeRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                //WarpClient.GetInstance().SendChat("hello");
            }
        }

        public void onUnSubscribeRoomDone(RoomEvent eventObj)
        {
        }

        public void onJoinRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                _page.showResult("joined room!");
                //WarpClient.GetInstance().SubscribeRoom(eventObj.getData().getId());
            }
            else
            {
                _page.showResult("failed to join room!");
            }
        }

        public void onLeaveRoomDone(RoomEvent eventObj)
        {
        }

        public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
        {
            _page.showResult("LiveRoominfo" + eventObj.getJoinedUsers().Length);
            GamePage.masItem = eventObj.getJoinedUsers().Length - 1;
            //Maxusers = int.Parse(eventObj.getData().getMaxUsers().ToString());
            GamePage.maxUsers = int.Parse(eventObj.getData().getMaxUsers().ToString());
            //RoomData d = new RoomData("1995630518","1","s", 3);
            //Balls.cPos = new Microsoft.Xna.Framework.Vector2[eventObj.getData().getMaxUsers()];
            //Balls.sPos = new Microsoft.Xna.Framework.Vector2[eventObj.getData().getMaxUsers()];
            //_page.showResult("Max" + eventObj.getData().getMaxUsers().ToString());
        }

        public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
        {
            //WarpClient.GetInstance().GetLiveRoomInfo(eventObj.getData().getId());
        }

        public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
        {
            if (WarpResponseResultCode.SUCCESS == eventObj.getResult())
            {
                _page.showResult("UpdateProperty event received with success status");
            }
            else
            {
                _page.showResult("Update Propert event received with fail status. Status is :" + eventObj.getResult().ToString());
            }
        }

    }
}
