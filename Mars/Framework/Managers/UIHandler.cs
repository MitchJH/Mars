using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars
{
    public static class UIHandler
    {
        static UIHandler()
        {
            // Note from Mitch for Mitch about how to approach the UI
            // The entire screen space (Viewport) for the main map needs to be a UI component. Then we have a stack of UI elements branching from that as the parent.
            // If a child in the branch is hit we know to ignore, it's parent, and it's parents parent, and so forth. This is how we ignore clicks 'through' the UI.
            // We only report a UI interaction like a click to a UI component layer after fully ascertaining where in the UI stack the interaction was meant to be.
            // With all of the above in place an interaction like a click is sent only to the relevant intended UI piece, every other UI piece is unaware of it.
        }
    }
}
