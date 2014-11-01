new GuiControl(MainMenuGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "1024 768";
   minExtent = "8 8";
   wrap = false;

   new GuiControl([ButtonContainer]) {
      profile = BackgroundProfile;
      horizSizing = "width";
      vertSizing = "height";
      position = "205 256";
      extent = "500 500";

      new GuiTextCtrl([Cursor]) {
         profile = TitleProfile;
         position = "0 0";
         text = ">";
      };

      new GuiControl([Buttons]) {
         extent = "500 500";
         selected = 0;

         new GuiTextCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            position = "20 0";
            text = "PLAY";
         };

         new GuiTextCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            position = "20 40";
            text = "JOIN";
         };

         new GuiTextCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            position = "20 80";
            text = "EXIT";
         };
      };
   };
};

function MainMenuGui::onWake(%this) {
   %this.updateCursor();
}

function MainMenuGui::updateCursor(%this) {
   %buttons = %this-->Buttons;
   %selectedPos = %buttons.getObject(%buttons.selected).position;
   %this-->Cursor.position = 0 SPC getWord(%selectedPos, 1);
}

InputEvents.subscribe(MainMenuGui, EvtNext);
function MainMenuGui::onEvtNext(%this) {
   %buttons = %this-->Buttons;
   %buttonCount = %buttons.getCount();
   if (%buttons.selected < %buttonCount-1) {
      %buttons.selected++;
      %this.updateCursor();
   }
}

InputEvents.subscribe(MainMenuGui, EvtPrev);
function MainMenuGui::onEvtPrev(%this) {
   %buttons = %this-->Buttons;
   %buttonCount = %buttons.getCount();
   if (%buttons.selected > 0) {
      %buttons.selected--;
      %this.updateCursor();
   }
}
