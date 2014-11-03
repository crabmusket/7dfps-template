new GuiControl(InGameMenuGui) {
   profile = DialogProfile;
   extent = "800 600";

   new GuiControl() {
      profile = BackgroundProfile;
      extent = "800 200";
      position = "0 200";

      new GuiTextCtrl([Cursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
      };

      new GuiControl([Buttons]) {
         profile = BackgroundProfile;
         extent = "800 200";

         new GuiButtonCtrl() {
            class = InGameMenuButton;
            profile = TitleProfile;
            position = "200 80";
            extent = "250 50";
            text = "LEAVE GAME";
            command = "GuiEvents.postEvent(EvtLeaveGame);";
            useMouseEvents = true;
         };

         new GuiButtonCtrl() {
            class = InGameMenuButton;
            profile = TitleProfile;
            position = "450 80";
            extent = "200 50";
            text = "CANCEL";
            command = "InputEvents.postEvent(EvtEscape);";
            useMouseEvents = true;
         };
      };
   };
};

function InGameMenuGui::onWake(%this) {
   %this.setSelected(1);
   InputEvents.subscribe(InGameMenuGui, EvtNext);
   InputEvents.subscribe(InGameMenuGui, EvtPrev);
   InputEvents.subscribe(InGameMenuGui, EvtAdvance);
}

function InGameMenuGui::onSleep(%this) {
   InputEvents.removeAll(%this);
}

function InGameMenuGui::updateCursor(%this) {
   %buttons = %this-->Buttons;
   %button = %buttons.getObject(%buttons.selected);
   %selectedPos = VectorSub(
      %button.position,
      0 SPC (getWord(%button.extent, 1) * 0.5)
   );
   %this-->Cursor.position = 0 SPC getWord(%selectedPos, 1);
}

function InGameMenuGui::setSelected(%this, %index) {
   %buttons = %this-->Buttons;
   if (%index >= 0 && %index < %buttons.getCount()) {
      %buttons.selected = %index;
      %this.updateCursor();
   }
}

function InGameMenuGui::getSelected(%this) {
   %buttons = %this-->Buttons;
   return %buttons.getObject(%buttons.selected);
}

function InGameMenuGui::onEvtNext(%this) {
   %this.setSelected(%this-->Buttons.selected + 1);
}

function InGameMenuGui::onEvtPrev(%this) {
   %this.setSelected(%this-->Buttons.selected - 1);
}

function InGameMenuGui::onEvtAdvance(%this) {
   eval(%this.getSelected().command);
}

function InGameMenuButton::onMouseEnter(%this) {
   InGameMenuGui.setSelected(InGameMenuGui-->Buttons.getObjectIndex(%this));
}
