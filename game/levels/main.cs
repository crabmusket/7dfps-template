new ScriptMsgListener(Levels) {
   // This global object will store an array of loaded levels that other modules
   // can make use of. The array contains key-value pairs of level title, and
   // level info object. No two level scan have the same title.
   levels = new ArrayObject();
};

new EventManager(LevelEvents) {
   queue = LevelEventQueue;
};
LevelEvents.registerEvent(EvtLevelRegistered);
LevelEvents.registerEvent(EvtLevelLoaded);

function Levels::getLevel(%this, %title) {
   return %this.levels.getValue(%this.levels.getIndexFromKey(%title));
}

function Levels::loadLevel(%this, %title) {
   %this.destroyLevel();
   %levelInfo = %this.levels.getValue(%title);
   if (isObject(%levelInfo)) {
      exec(%levelInfo.file);
      new SimGroup(LevelCleanup);
      LevelEvents.postEvent(EvtLevelLoaded);
   }
}

GameEvents.subscribe(Levels, EvtExit);
function Levels::onEvtExit(%this) {
   %this.destroyLevel();
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
function Levels::onEvtStart(%this, %args) {
   // When the game starts, we'll scan for level files in the levels/ directory
   // and all subdirectories. A 'level' file is anything named with a .mis
   // extension.
   %pattern = "levels/*.mis";
   for (%f = findFirstFile(%pattern); %f !$= ""; %f = findNextFile()) {
      // Extract the level info from the file and register it in our 'database'.
      %levelInfo = %this.getLevelInfo(%f);
      if (%levelInfo != 0) {
         // We assume image files will be named identically to the level files
         // they go with.
         %levelInfo.image = filePath(%f) @ "/" @ fileBase(%f);
         %levelInfo.file = %f;
         %this.register(%levelInfo);
      }
   }
}

function Levels::register(%this, %level) {
   // Check first for levels with duplicate titles, since title is how we'll
   // identify a level.
   if (%this.getLevel(%level.levelName) !$= "") {
      error("Level title '"@%level.levelName@"' is already taken!");
   } else {
      %this.levels.push_back(%level.levelName, %level);
      LevelEvents.postEvent(EvtLevelRegistered, %level);
   }
}

function Levels::getLevelInfo(%this, %filename) {
   // We're going to parse the file textually, searching for the LevelInfo
   // object declaration. This means we avoid executing the whole file and
   // creating a bunch of objects.
   %file = new FileObject();
   %levelInfoStr = "";

   if (%file.openForRead(%filename)) {
      // Read line-by-line through the file until we get to the info block.
      // All lines that are part of the info block will be appended to
      // %levelInfoStr.
      %inInfoBlock = false;

      while (!%file.isEOF()) {
         %line = trim(%file.readLine());
         if (startsWith(%line, "new LevelInfo(")) {
            %inInfoBlock = true;
         } else if (%inInfoBlock && %line $= "};") {
            %inInfoBlock = false;
            %levelInfoStr = %levelInfoStr SPC %line;
            break;
         }

         if (%inInfoBlock) {
            %levelInfoStr = %levelInfoStr SPC %line;
         }
      }

      %file.close();
   }
   %file.delete();

   if (%levelInfoStr !$= "") {
      %levelInfoStr = "%levelInfoObj = " @ %levelInfoStr;
      eval(%levelInfoStr);
      %obj = new ScriptObject() {
         levelName = %levelInfoObj.levelName;
         description = %levelInfoObj.description;
      };
      %levelInfoObj.delete();
      return %obj;
   }

   // Didn't find a LevelInfo :(.
   return 0;
}
