new GuiControl(SelectServerGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "1024 768";
   minExtent = "8 8";
   wrap = false;

   new GuiTextCtrl() {
      profile = TitleProfile;
      text = "CHOOSE SERVER";
      position = "100 80";
      extent = "250 30";
   };

   new GuiControl([ServerSelect]) {
      position = "100 170";
      extent = "500 500";

      new GuiTextCtrl([ServerCursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
         visible = false;
      };

      new GuiStackControl([Servers]) {
         position = "20 0";
         stackingType = "Vertical";
         dynamicSize = true;
         dynamicNonStackExtent = true;
         padding = 5;
         extent = "280 480";
         selected = 0;
      };
   };

   new GuiTextCtrl([JoinCursor]) {
      profile = TitleProfile;
      position = "580 485";
      horizSizing = Left;
      vertSizing = Top;
      text = ">";
      visible = false;
   };

   new GuiButtonCtrl([JoinButton]) {
      class = JoinButton;
      profile = TitleProfile;
      text = "PLAY";
      position = "600 500";
      horizSizing = Left;
      vertSizing = Top;
      command = "GuiEvents.postEvent(EvtJoinGame);";
      useMouseEvents = true;
   };
};

function SelectServerGui::onWake(%this) {
   HTTPMaster.query(%this, queryDone);
   InputEvents.subscribe(%this, EvtPrev);
   InputEvents.subscribe(%this, EvtNext);
   InputEvents.subscribe(%this, EvtForwards);
   InputEvents.subscribe(%this, EvtBackwards);
   InputEvents.subscribe(%this, EvtAdvance);
}

function SelectServerGui::onSleep(%this) {
   InputEvents.removeAll(%this);
}

function SelectServerGui::queryDone(%this, %query) {
   %list = HTTPMaster-->ServerList;
   %list.clearItems();
   %servers = getRecordCount(%query.contents);
   if (%servers > 0) {
   } else {
      // Put up a message somehow
   }
}

function SelectServerGui::addServer(%this, %info) {
   %servers = %this-->Servers;
   %servers.add(new GuiButtonCtrl() {
      class = LevelListButton;
      profile = SmallTitleProfile;
      text = %info;
      command = "GuiEvents.postEvent(EvtSelectServer, \""@%level.levelName@"\");";
      useMouseEvents = true;
   });
}

function ServerListButton::onWake(%this) {
   GuiEvents.subscribe(%this, EvtSelectServer);
}
function ServerListButton::onSleep(%this) {
   GuiEvents.removeAll(%this);
}
function ServerListButton::onEvtSelectLevel(%this, %title) {
   if (%this.text $= %title) {
      %this.profile = SmallTitleProfileInverted;
   } else {
      %this.profile = SmallTitleProfile;
   }
}
function ServerListButton::onMouseEnter(%this) {
   SelectServerGui.cursorToServer(SelectServerGui-->Servers.getObjectIndex(%this));
}

function SelectServerGui::cursorToServer(%this, %idx) {
   %servers = %this-->Servers;
   if (%idx < 0 || %idx == %servers.getCount()) {
      return;
   }
   %servers.selected = %idx;
   %server = %servers.getObject(%idx);
   %selectedPos = VectorSub(
      %server.position,
      0 SPC (getWord(%server.extent, 1) * 0.5)
   );
   %this-->ServerCursor.position = 0 SPC getWord(%selectedPos, 1);

   %this-->ServerCursor.visible = true;
   %this-->JoinCursor.visible = false;
}

function SelectServerGui::cursorToJoinButton(%this) {
   %this-->ServerCursor.visible = false;
   %this-->JoinCursor.visible = true;
}

function SelectServerGui::onEvtNext(%this) {
   %servers = %this-->Servers;
   if (%servers.selected == %servers.getCount() - 1) {
      %this.cursorToPlayButton();
   } else {
      %this.cursorToServer(%servers.selected + 1);
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

function JoinButton::onMouseEnter(%this) {
   SelectServerGui.cursorToJoinButton();
}
