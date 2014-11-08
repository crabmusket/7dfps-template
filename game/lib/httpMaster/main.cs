new ScriptObject(HTTPMaster) {
   masterLocation = "localhost:3000";
   gameListURI = "/games";
   gameUpdateURI = "/games";
   heartbeatPeriod = 60;
};

new EventManager(HTTPMasterEvents) {
   queue = HTTPMasterEventQueue;
};
HTTPMasterEvents.registerEvent(EvtQueryDone);
HTTPMasterEvents.registerEvent(EvtQueryError);

function HTTPMaster::query(%this, %callbackObj, %callback) {
   %http = new HTTPObject() {
      class = HTTPMasterQuery;
   };
   %http.get(%this.masterLocation, %this.gameListURI, "");
   if (%callback !$= "") {
      // If we have both callback parameters, they work as it says on the tin.
      %http.callbackObj = %callbackObj;
      %http.callback = %callback;
   } else if (%callbackObj !$= "") {
      // If we only have one parameter, treat it as the function name.
      %http.callback = %callbackObj;
   }
   return %this;
}

function HTTPMasterQuery::onLine(%this) {
   %this.contents = %this.contents @ %line;
}

function HTTPMasterQuery::onDisconect(%this) {
   if (isObject(%this.callbackObj)) {
      %this.callbackObj.call(%this.callback, %this);
   } else if(%this.callback !$= "") {
      call(%this.callback, %this);
   } else {
      HTTPMasterEvents.postEvent(EvtQueryDone, %this);
   }
}

function HTTPMaster::startHeartbeat(%this) {
   %this._sendHeartbeat();
   return %this;
}

function HTTPMaster::_sendHeartbeat(%this) {
   %this.heartbeat = %this.schedule(%this.heartbeatPeriod * 1000, _sendHeartbeat);
   %http = new HTTPObject() {
      class = HTTPMasterUpdate;
   };
   %http.post(%this.masterLocation, %this.gameUpdateURI, "", "");
}

function HTTPMasterUpdate::onDisconnect(%this) {
   %this.schedule(1, delete);
}

function HTTPMaster::stopHeartbeat(%this) {
   if (%this.heartbeat !$= "") {
      cancel(%this.heartbeat);
   }
   return %this;
}
