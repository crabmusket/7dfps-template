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
         };

         new GuiButtonCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            text = "JOIN";
            useMouseEvents = true;
         };

         new GuiButtonCtrl() {
            class = MainMenuButton;
            profile = TitleProfile;
            text = "EXIT";
            useMouseEvents = true;
         };
      };
   };
};

function MainMenuGui::onWake(%this) {
   %this.setSelected(0);
}

function MainMenuGui::setSelected(%this, %index) {
   %buttons = %this-->Buttons;
   if (%index >= 0 && %index < %buttons.getCount()) {
      %buttons.selected = %index;
      %this.updateCursor();
   }
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

function MainMenuButton::onMouseEnter(%this) {
   MainMenuGui.setSelected(MainMenuGui-->Buttons.getObjectIndex(%this));
}
