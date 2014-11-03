new ScriptObject(NetClient) {
   port = 28002;
};

new EventManager(NetClientEvents) {
   queue = NetClientEventQueue;
};

function NetClient::connectTo(%this, %host, %port) {
   %this.disconnect();
   %this.connection = new GameConnection();

   if (%host $= self) {
      %this.connection.connectLocal();
   } else {
      // Set the local port we connect from.
      setNetPort(%this.port);
      // Connect to remote host.
      %this.connection.connect(%host @ (%port !$= "" ? (":" @ %port) : ""));
   }

   return %this;
}

function NetClient::disconnect(%this) {
   if (isObject(%this.connection)) {
      %this.connection.delete();
      NetClientEvents.postEvent(EvtDisconnected);
   }
}

NetClientEvents.registerEvent(EvtDisconnected);

foreach$ (%evt in DatablocksDone
              SPC DatablockObjectReceived
              SPC InitialControlSet
              SPC ConnectionError
              SPC ConnectRequestRejected
              SPC ConnectionDropped
              SPC ControlObjectChange
              SPC ConnectionTimedOut
              SPC ConnectionAccepted
              SPC ConnectRequestTimedOut) {
   NetClientEvents.registerEvent(Evt @ %evt);
}

function GameConnection::initialControlSet(%this) {
   NetClientEvents.postEvent(EvtInitialControlSet);
}

function GameConnection::onDatablockObjectReceived(%this, %num, %total) {
   NetClientEvents.postEvent(EvtDatablockObjectReceived, %num SPC %total);
}

foreach$ (%evt in ControlObjectChange
              SPC ConnectionTimedOut
              SPC ConnectionAccepted
              SPC ConnectRequestTimedOut) {
   eval(
"function GameConnection::on"@%evt@"(%this) {" @
"   NetClientEvents.postEvent(Evt"@%evt@");" @
"}"
   );
}

foreach$ (%evt in DatablocksDone
              SPC ConnectionError
              SPC ConnectRequestRejected
              SPC ConnectionDropped) {
   eval(
"function GameConnection::on"@%evt@"(%this, %data) {" @
"   NetClientEvents.postEvent(Evt"@%evt@", %data);" @
"}"
   );
}
