new ScriptMsgListener(Levels) {
   levels = new ArrayObject();
};

new EventManager(LevelEvents) {
   queue = LevelEventQueue;
};
LevelEvents.registerEvent(EvtLevelRegistered);

function Levels::getLevel(%this, %title) {
   return %this.levels.getValue(%this.levels.getIndexFromKey(%title));
}

function Levels::register(%this, %level) {
   if (%this.getLevel(%level.title) !$= "") {
      error("Level title '"@%level.title@"' is already taken!");
   } else {
      %this.levels.push_back(%level.title, %level);
      LevelEvents.postEvent(EvtLevelRegistered, %level);
   }
}

function Levels::loadLevel(%this, %title) {
   %this.destroyLevel();
   %level = %this.levels.getValue(%title);
   if (isObject(%level) && %level.file !$= "") {
      exec(%level.file);
      new SimGroup(LevelCleanup);
   }
}

function Levels::destroyLevel(%this) {
   if (isObject(LevelGroup)) {
      LevelGroup.delete();
   }
   if (isObject(LevelCleanup)) {
      LevelCleanup.delete();
   }
}

GameEvents.subscribe(Levels, EvtStart);
function Levels::onEvtStart(%this) {
   %dirs = getDirectoryList("levels");
   for (%f = 0; %f < getFieldCount(%dirs); %f++) {
      %dir = getField(%dirs, %f);
      exec("levels/" @ %dir @ "/main.cs");
   }
}

GameEvents.subscribe(Levels, EvtExit);
function Levels::onEvtExit(%this) {
   %this.destroyLevel();
}
