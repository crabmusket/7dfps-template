new GuiControl(SelectLevelGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "800 600";
   minExtent = "8 8";
   wrap = false;

   new GuiTextCtrl() {
      profile = TitleProfile;
      text = "CHOOSE LEVEL";
      position = "20 20";
      extent = "250 30";
   };

   new GuiButtonCtrl() {
      profile = TitleProfile;
      text = "PLAY";
      position = "20 70";
      command = "GuiEvents.postEvent(EvtStartGame);";
   };

   new GuiStackControl([Levels]) {
      position = "20 120";
      stackingType = "Vertical";
      dynamicSize = true;
      dynamicNonStackExtent = true;
      padding = 5;
      extent = "360 480";
   };
};

GuiEvents.registerEvent(EvtStartGame);
GuiEvents.registerEvent(EvtSelectLevel);

function SelectLevelGui::onWake(%this) {
   %levels = %this-->Levels;
   if (%levels.getCount() > 0) {
      GuiEvents.postEvent(EvtSelectLevel, %levels.getObject(0).text);
   }
}

function SelectLevelGui::addLevel(%this, %level) {
   %levels = %this-->Levels;
   %levels.add(new GuiButtonCtrl() {
      class = LevelListButton;
      profile = LevelListProfile;
      text = %level.title;
      command = "GuiEvents.postEvent(EvtSelectLevel, \""@%level.title@"\");";
   });
}

function LevelListButton::onAdd(%this) {
   GuiEvents.subscribe(%this, EvtSelectLevel);
}

function LevelListButton::onEvtSelectLevel(%this, %level) {
   if (%this.text $= %level) {
      %this.profile = LevelListInvertedProfile;
   } else {
      %this.profile = LevelListProfile;
   }
}
