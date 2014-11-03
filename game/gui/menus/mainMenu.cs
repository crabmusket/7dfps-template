new GuiControl(MainMenuGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "800 400";
   minExtent = "8 8";
   wrap = false;

   new GuiControl([ButtonContainer]) {
      profile = BackgroundProfile;
      horizSizing = "width";
      vertSizing = "height";
      position = "100 200";
      extent = "500 500";

      new GuiTextCtrl([Cursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
      };

      new GuiStackControl([Buttons]) {
         position = "20 0";
         stackingType = "Vertical";
         dynamicSize = true;
         dynamicNonStackExtent = true;
         padding = 15;
         selected = 0;

         new GuiButtonCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            text = "PLAY";
            useMouseEvents = true;
            command = "GuiEvents.postEvent(EvtNewGame);";
         };

         new GuiButtonCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            text = "JOIN";
            useMouseEvents = true;
            command = "GuiEvents.postEvent(EvtJoinGame);";
         };

         new GuiButtonCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            text = "EXIT";
            useMouseEvents = true;
            command = "quit();";
         };
      };
   };
};

function MainMenuGui::onWake(%this) {
   %this.updateCursor();
   InputEvents.subscribe(MainMenuGui, EvtNext);
   InputEvents.subscribe(MainMenuGui, EvtPrev);
   InputEvents.subscribe(MainMenuGui, EvtAdvance);
}

function MainMenuGui::onSleep(%this) {
   InputEvents.removeAll(%this);
}

function MainMenuGui::setSelected(%this, %index) {
   %buttons = %this-->Buttons;
   if (%index >= 0 && %index < %buttons.getCount()) {
      %buttons.selected = %index;
      %this.updateCursor();
   }
}

function MainMenuGui::getSelected(%this) {
   %buttons = %this-->Buttons;
   return %buttons.getObject(%buttons.selected);
}

function MainMenuGui::updateCursor(%this) {
   %buttons = %this-->Buttons;
   %button = %buttons.getObject(%buttons.selected);
   %selectedPos = VectorSub(
      %button.position,
      0 SPC (getWord(%button.extent, 1) * 0.5)
   );
   %this-->Cursor.position = 0 SPC getWord(%selectedPos, 1);
}

function MainMenuGui::onEvtNext(%this) {
   %this.setSelected(%this-->Buttons.selected + 1);
}

function MainMenuGui::onEvtPrev(%this) {
   %this.setSelected(%this-->Buttons.selected - 1);
}

function MainMenuGui::onEvtAdvance(%this) {
   eval(%this.getSelected().command);
}

function MainMenuButton::onMouseEnter(%this) {
   MainMenuGui.setSelected(MainMenuGui-->Buttons.getObjectIndex(%this));
}
