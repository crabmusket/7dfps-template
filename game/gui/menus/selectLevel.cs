new GuiControl(SelectLevelGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "800 600";
   minExtent = "8 8";
   wrap = false;
   selectedCtrl = "";

   new GuiControl([LevelSelect]) {
      position = "100 100";
      extent = "500 500";

      new GuiTextCtrl() {
         profile = TitleProfile;
         text = "CHOOSE LEVEL";
         position = "20 0";
         extent = "250 30";
      };

      new GuiTextCtrl([Cursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
      };

      new GuiStackControl([Levels]) {
         position = "20 100";
         stackingType = "Vertical";
         dynamicSize = true;
         dynamicNonStackExtent = true;
         padding = 5;
         extent = "280 480";
      };
   };

   new GuiStackControl([LevelInfo]) {
      position = "440 100";
      stackingType = "Vertical";
      dynamicSize = true;
      dynamicNonStackExtent = true;
      padding = 15;
      extent = "300 500";

      new GuiBitmapCtrl([LevelImage]) {
         profile = BackgroundProfile;
         extent = "300 200";
      };

      new GuiMLTextCtrl([LevelDescription]) {
         profile = TextProfile;
         extent = "300 300";
      };
   };

   new GuiButtonCtrl() {
      profile = TitleProfile;
      text = "PLAY";
      position = "600 500";
      horizSizing = Left;
      vertSizing = Top;
      command = "GuiEvents.postEvent(EvtStartGame);";
   };
};

function SelectLevelGui::onWake(%this) {
   GuiEvents.subscribe(%this, EvtSelectLevel);
   %levels = %this-->Levels;
   if (%levels.getCount() > 0) {
      GuiEvents.postEvent(EvtSelectLevel, %levels.getObject(0).text);
   }
}

function SelectLevelGui::onEvtSelectLevel(%this, %title) {
   %level = Levels.getLevel(%title);
   if (isObject(%level)) {
      %this-->LevelImage.setBitmap(%level.image);
      %this-->LevelDescription.setText(%level.description);
   }
}

function SelectLevelGui::onSleep(%this) {
   GuiEvents.removeAll(%this);
}

function SelectLevelGui::addLevel(%this, %level) {
   %levels = %this-->Levels;
   %levels.add(new GuiButtonCtrl() {
      class = LevelListButton;
      profile = LevelListProfile;
      text = %level.levelName;
      command = "GuiEvents.postEvent(EvtSelectLevel, \""@%level.levelName@"\");";
   });
}

function LevelListButton::onWake(%this) {
   GuiEvents.subscribe(%this, EvtSelectLevel);
}
function LevelListButton::onSleep(%this) {
   GuiEvents.removeAll(%this);
}
function LevelListButton::onEvtSelectLevel(%this, %title) {
   if (%this.text $= %title) {
      %this.profile = LevelListInvertedProfile;
   } else {
      %this.profile = LevelListProfile;
   }
}
