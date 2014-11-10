new GuiControl(SelectLevelGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "800 600";
   minExtent = "8 8";
   wrap = false;
   selectedCtrl = "";

   new GuiTextCtrl() {
      profile = TitleProfile;
      text = "CHOOSE LEVEL";
      position = "120 100";
      extent = "250 30";
   };

   new GuiControl([LevelSelect]) {
      position = "100 170";
      extent = "500 500";

      new GuiTextCtrl([LevelCursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
         visible = false;
      };

      new GuiStackControl([Levels]) {
         position = "20 0";
         stackingType = "Vertical";
         dynamicSize = true;
         dynamicNonStackExtent = true;
         padding = 5;
         extent = "280 480";
         selected = 0;
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

   new GuiTextCtrl([PlayCursor]) {
      profile = TitleProfile;
      position = "580 485";
      horizSizing = Left;
      vertSizing = Top;
      text = ">";
      visible = false;
   };

   new GuiButtonCtrl([PlayButton]) {
      class = PlayButton;
      profile = TitleProfile;
      text = "PLAY";
      position = "600 500";
      horizSizing = Left;
      vertSizing = Top;
      command = "GuiEvents.postEvent(EvtStartGame);";
      useMouseEvents = true;
   };
};

function SelectLevelGui::onWake(%this) {
   GuiEvents.subscribe(%this, EvtSelectLevel);
   InputEvents.subscribe(%this, EvtPrev);
   InputEvents.subscribe(%this, EvtNext);
   InputEvents.subscribe(%this, EvtForwards);
   InputEvents.subscribe(%this, EvtBackwards);
   InputEvents.subscribe(%this, EvtAdvance);

   %levels = %this-->Levels;
   if (%levels.getCount() > 0) {
      %levels.sort(SelectLevelGuiSortCallback);
      %levels.updateStack();
      GuiEvents.postEvent(EvtSelectLevel, %levels.getObject(0).text);
      %this.cursorToLevel(0);
   } else {
      // Well this is tricky.
   }
}

function SelectLevelGui::onSleep(%this) {
   GuiEvents.removeAll(%this);
   InputEvents.removeAll(%this);
}

function SelectLevelGuiSortCallback(%a, %b) {
   if (%a.sortOrder < %b.sortOrder) {
      return -1;
   } else if (%a.sortOrder > %b.sortOrder) {
      return 1;
   } else {
      return 0;
   }
}

function SelectLevelGui::cursorToLevel(%this, %idx) {
   %levels = %this-->Levels;
   if (%idx < 0 || %idx == %levels.getCount()) {
      return;
   }
   %levels.selected = %idx;
   %level = %levels.getObject(%idx);
   %selectedPos = VectorSub(
      %level.position,
      0 SPC (getWord(%level.extent, 1) * 0.5)
   );
   %this-->LevelCursor.position = 0 SPC getWord(%selectedPos, 1);

   %this-->LevelCursor.visible = true;
   %this-->PlayCursor.visible = false;
}

function SelectLevelGui::cursorToPlayButton(%this) {
   %this-->LevelCursor.visible = false;
   %this-->PlayCursor.visible = true;
}

function SelectLevelGui::onEvtNext(%this) {
   %levels = %this-->Levels;
   if (%levels.selected == %levels.getCount() - 1) {
      %this.cursorToPlayButton();
   } else {
      %this.cursorToLevel(%levels.selected + 1);
   }
}

function SelectLevelGui::onEvtPrev(%this) {
   %levels = %this-->Levels;
   if (%this-->LevelCursor.visible) {
      %this.cursorToLevel(%levels.selected - 1);
   } else if (%this-->PlayCursor.visible) {
      %this.cursorToLevel(%levels.selected);
   }
}

function SelectLevelGui::onEvtForwards(%this) {
   %this.cursorToPlayButton();
}

function SelectLevelGui::onEvtBackwards(%this) {
   %this.cursorToLevel(%this-->Levels.selected);
}

function SelectLevelGui::onEvtAdvance(%this) {
   if (%this-->LevelCursor.visible) {
      %obj = %this-->Levels.getObject(%this-->Levels.selected);
   } else if (%this-->PlayCursor.visible) {
      %obj = %this-->PlayButton;
   }
   if (%obj) {
      %obj.schedule(1, performClick);
   }
}

function SelectLevelGui::onEvtSelectLevel(%this, %title) {
   %level = Levels.getLevel(%title);
   if (isObject(%level)) {
      %this-->LevelImage.setBitmap(%level.image);
      %this-->LevelDescription.setText(%level.description);
   }
}

function SelectLevelGui::addLevel(%this, %level) {
   %levels = %this-->Levels;
   %levels.add(new GuiButtonCtrl() {
      class = LevelListButton;
      profile = SmallTitleProfile;
      text = %level.levelName;
      command = "GuiEvents.postEvent(EvtSelectLevel, \""@%level.levelName@"\");";
      useMouseEvents = true;
      sortOrder = %level.sortOrder;
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
      %this.profile = SmallTitleProfileInverted;
   } else {
      %this.profile = SmallTitleProfile;
   }
}
function LevelListButton::onMouseEnter(%this) {
   SelectLevelGui.cursorToLevel(SelectLevelGui-->Levels.getObjectIndex(%this));
}

function PlayButton::onMouseEnter(%this) {
   SelectLevelGui.cursorToPlayButton();
}
function PlayButton::onClick(%this) {
   InputEvents.setInputMethod(keyboard);
}
