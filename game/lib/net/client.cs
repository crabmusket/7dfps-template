new ScriptObject(NetClient) {
   port = 7001;
};

function NetClient::init(%this) {
   return %this;
}

function NetClient::connectTo(%this, %host, %port) {
   %this.disconnect();
   %this.connection = new GameConnection();

   if (%host $= self) {
      %this.connection.connectLocal();
   } else {
      setNetPort(%this.port);
      %this.connection.connect(%host @ (%port !$= "" ? (":" @ %port) : ""));
   }

   return %this;
}

function NetClient::disconnect(%this) {
   if(isObject(%this.connection)) {
      %this.connection.delete();
   }
}

function NetClient::destroy(%this) {
   %this.disconnect();
   %this.delete();
}
