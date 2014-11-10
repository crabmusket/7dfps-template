GameViewGui.add(new GuiControl(HUD) {
   profile = InvisibleProfile;
   horizSizing = Right;
   vertSizing = Bottom;
   position = "0 0";
   extent = "800 600";
   visible = false;

   new GuiControl([Interaction]) {
      profile = BackgroundProfile;
      position = "-350 50";
      extent = "350 50";

      new GuiControl([InteractionIcon]) {
         position = "55 5";
         extent = "40 40";

         new GuiBitmapCtrl([Controller]) {
            visible = false;
            position = "5 5";
            extent = "30 30";
            bitmap = "gui/prompts/xbox360/360_X";
         };

         new GuiControl([KBM]) {
            visible = false;
            new GuiBitmapCtrl() {
               extent = "40 40";
               bitmap = "gui/prompts/kbm/blanks/Blank_White_Normal";
            };
            new GuiTextCtrl() {
               profile = BlackTextProfile;
               position = "15 0";
               extent = "40 40";
               text = "E";
            };
         };
      };

      new GuiTextCtrl([InteractionString]) {
         profile = SmallTitleProfile;
         position = "100 10";
         extent = "300 30";
      };
   };
});

function HUD::onWake(%this) {
   InputEvents.subscribe(%this, EvtChangeInputMethod);
   %this.onEvtChangeInputMethod(InputEvents.inputMethod);
}

function HUD::onSleep(%this) {
   InputEvents.removeAll(%this);
}

function HUD::onEvtChangeInputMethod(%this, %method) {
   HUD-->Controller.visible = (%method $= gamepad);
   HUD-->KBM.visible = (%method !$= gamepad);
}

HUD.Tweens = Twillex::create();
HUD.Tweens.startUpdates();

function clientCmdInteraction(%text) {
   HUD-->InteractionString.setText(%text);
   %textW = getWord(HUD-->InteractionString.extent, 0);
   HUD-->Interaction.extent = 120 + %textW SPC 50;
   HUD.Tweens.toOnce(200, HUD-->Interaction, "position: 0 50", "ease:sine_out");
}

function clientCmdNoInteraction() {
   %pos = -1 * getWord(Hud-->Interaction.extent, 0) SPC 50;
   HUD.Tweens.toOnce(200, HUD-->Interaction, "position:" SPC %pos, "ease:sine_in");
}
