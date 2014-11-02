new ScriptMsgListener(Levels) {
};

GameEvents.subscribe(Levels, EvtStart);
function Levels::onEvtStart(%this) {
   %dirs = getDirectoryList("levels");
   for (%f = 0; %f < getFieldCount(%dirs); %f++) {
      %dir = getField(%dirs, %f);
      exec("levels/" @ %dir @ "/main.cs");
   }
}
