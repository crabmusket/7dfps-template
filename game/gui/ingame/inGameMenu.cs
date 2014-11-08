new GuiControl(InGameMenuGui) {
   profile = DialogProfile;
   extent = "800 600";

   new GuiControl() {
      profile = BackgroundProfile;
      extent = "800 200";
      position = "0 200";

      new GuiControl([Buttons]) {
         profile = BackgroundProfile;
         extent = "800 200";

         new GuiButtonCtrl() {
            class = InGameMenuButton;
            profile = TitleProfile;
            position = "190 80";
            extent = "250 30";
            text = "LEAVE GAME";
            command = "GuiEvents.postEvent(EvtLeaveGame);";
            useMouseEvents = true;
         };

         new GuiButtonCtrl() {
            class = InGameMenuButton;
            profile = TitleProfile;
            position = "450 80";
            extent = "190 30";
            text = "CANCEL";
            command = "InputEvents.postEvent(EvtEscape);";
            useMouseEvents = true;
         };
      };

      new GuiTextCtrl([Cursor]) {
         profile = TitleProfile;
         text = ">";
      };
   };
};

function InGameMenuGui::onWake(%this) {
   %this.setSelected(1);
   InputEvents.subscribe(InGameMenuGui, EvtForwards);
   InputEvents.subscribe(InGameMenuGui, EvtBackwards);
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
      20 SPC (getWord(%button.extent, 1) * 0.5)
   );
   %this-->Cursor.position = getWords(%selectedPos, 0, 1);
   error(%this-->Cursor.position);
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

function InGameMenuGui::onEvtForwards(%this) {
   %this.setSelected(%this-->Buttons.selected + 1);
}

function InGameMenuGui::onEvtBackwards(%this) {
   %this.setSelected(%this-->Buttons.selected - 1);
}

function InGameMenuGui::onEvtAdvance(%this) {
   // To simulate an actual mouse click, we need to escape the event-handling
   // loop using a schedule to defer the click.
   %this.getSelected().schedule(1, performClick);
}

function InGameMenuButton::onMouseEnter(%this) {
   InGameMenuGui.setSelected(InGameMenuGui-->Buttons.getObjectIndex(%this));
}
