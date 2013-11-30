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
    public class ZoneReqListener : com.shephertz.app42.gaming.multiplayer.client.listener.ZoneRequestListener
    {
        private MainPage _page;

        public ZoneReqListener(MainPage page)
        {
            _page = page;
        }

        public void onDeleteRoomDone(RoomEvent eventObj)
        {
        }
        public void onGetAllRoomsDone(AllRoomsEvent eventObj)
        {
            _page.showResult("rooms are "+ eventObj.getRoomIds().Length);
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
    }
}
