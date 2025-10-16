using System.Collections.Generic;
using TMPro;

namespace Dialogs
{
    public class DialoguesDataBase
    {
        public Dictionary<string, TextMeshProUGUI> dialogues = new()
        {
            { "ElderlyCouple_Main", ElderlyCouple_Main },
            { "ElderlyCouple_Positive", ElderlyCouple_Positive },
            { "ElderlyCouple_Negative", ElderlyCouple_Negative },
            { "Child_Main", Child_Main },
            { "Child_Positive", Child_Positive },
            { "Child_Negative", Child_Negative },
            { "ShoeSeller_Main", ShoeSeller_Main },
            { "ShoeSeller_Positive", ShoeSeller_Positive },
            { "ShoeSeller_Negative", ShoeSeller_Negative },
            { "Mailman_Main", Mailman_Main },
            { "Mailman_Positive", Mailman_Positive },
            { "Mailman_Negative", Mailman_Negative },
            { "StrayCat_Main", StrayCat_Main },
            { "StrayCat_Positive", StrayCat_Positive },
            { "StrayCat_Negative", StrayCat_Negative },
            { "TrafficOfficer_Main", TrafficOfficer_Main },
            { "TrafficOfficer_Positive", TrafficOfficer_Positive },
            { "TrafficOfficer_Negative", TrafficOfficer_Negative },
            { "StartDialogue", StartDialogue },
            { "EndDialogue", EndDialogue },
        };

        #region SartEnd

        private static TextMeshProUGUI StartDialogue = new ()
        {
            text = 
                "I have to make it back on time, but the only way to do that is to cut through the cemetery…" +
                "\nNothing bad could really happen, right? They always say it’s the living you should fear, not the dead." +
                "\nEither way, the more I think about it, the less time I’ve got. And there’s no choice left.\n"
        };
        private static TextMeshProUGUI EndDialogue = new ()
        {
            text = 
                "<color=green>“Finally, an exit!”</color>" +
                "\nI paused a moment at the cemetery gates to catch my breath. I don’t know why, but I had the feeling I’d walked the edge countless times." +
                "\nYeah… that was quite a walk — I’m never going through this cemetery again.\n"
        };

        #endregion
        
        
        #region ElderlyCouple

        private static TextMeshProUGUI ElderlyCouple_Main = new ()
        {
            text =
                "A strange old couple stood right in the middle of the graves and seemed to be… digging? With pitchforks?!" +
                "\nWell, the man was digging, while the woman just paced around him, loudly complaining." +
                "\n<color=red>Old Woman: “I told you we should’ve brought the rake! You’re spilling it all over the place!”</color>" +
                "\nThe moment she noticed me, she stopped mid-sentence and flashed a wide, toothy smile." +
                "\n<color=red>Old Woman: “Oh, sweetheart, why don’t you settle this for us — what’s better for digging, huh?” </color>"
        };

        private static TextMeshProUGUI ElderlyCouple_Positive = new ()
        {
            text =
                "The old woman’s face dimmed instantly. She looked away, as if suddenly ashamed of something." +
                "\n<color=red>Old Woman: “Oh, is that so? You really think so?”</color>" +
                "\n<color=red>ld Woman: “Fine then, keep digging.”</color>" +
                "\nShe turned back to the old man, clearly losing all interest in me. I figured I’d better leave before they asked me anything else.\n"
        };

        private static TextMeshProUGUI ElderlyCouple_Negative = new ()
        {
            text = "<color=red>Old Man: “I’ve had enough of all of you!”</color>" +
                   "\n\nIn the next second, the muddy pitchfork went straight through the old woman’s stomach as if through butter. And before I could even gasp — through mine as well. The furious face of the old man, driven beyond reason, was the last thing I saw before everything went dark.\n"
        };

        #endregion ElderlyCouple

        #region Child

        private static TextMeshProUGUI Child_Main = new ()
        {
            text =
                "Out of the darkness, a child suddenly ran toward me — completely barefoot, clutching a big stuffed bear, which he immediately held out to me." +
                "<color=red>\nChild: “Will you play with me?”\n</color>"
        };

        private static TextMeshProUGUI Child_Positive = new ()
        {
            text =
                "To my surprise, we actually had a pretty good time! The kid was sharp for his age — reminded me a bit of my younger brother." +
                " When he finally got bored, he just wandered off like nothing had happened. Feeling oddly uplifted, I went on my way."
        };

        private static TextMeshProUGUI Child_Negative = new ()
        {
            text = "I instinctively took a step back until my heel caught on a root creeping across the ground." +
                   " I lost my balance and crashed down hard, the back of my head slamming into the cold, damp soil. " +
                   "Warm blood pooled beneath me, spreading fast. The last thing I heard was the child’s quiet sobbing."
        };

        #endregion Child

        #region Shoe Seller

        private static TextMeshProUGUI ShoeSeller_Main = new ()
        {
            text = "Through the thick fog, I spotted a rather unusual man: dressed in a sharp suit, " +
                   "he was holding a brand-new pair of shoes, stroking them with a tenderness usually reserved for a kitten." +
                   "\nA shoe seller, maybe? There are plenty of those in town, but… here? Now?" +
                   "\n<color=red>Seller: “Ah, good day, sir. Would you like to try them on?”</color>" +
                   "\nAnd then, suddenly, he offered them to me…\n"
        };

        private static TextMeshProUGUI ShoeSeller_Positive = new ()
        {
            text =
                "I just nodded and hurried around the stranger. Sure, a new pair of shoes would’ve been nice," +
                " but this guy looked far too suspicious."
        };

        private static TextMeshProUGUI ShoeSeller_Negative = new ()
        {
            text = "Deciding I should take them while I could, I eagerly extended my foot. Within seconds… a " +
                   "sharp, burning pain shot through me. A huge bear trap had clamped onto my ankle, and I " +
                   "lost consciousness instantly."
        };

        #endregion Shoe Seller
        
        #region Mailman

        private static TextMeshProUGUI Mailman_Main = new ()
        {
            text = 
                "Among the graves wandered a man, surrounded by pigeons. " +
                "He paced anxiously back and forth, squinting at the tombstones. " +
                "But when he saw me, his panic vanished instantly. " +
                "Fishing a small box from his pocket, he held it out to me, and I noticed the logo of the local postal service on his sleeve." +
                "\n<color=red>Mailman: “A package for you!”</color>" +
                "\nBut I knew for sure — nobody had sent me any packages…\n"
        };

        private static TextMeshProUGUI Mailman_Positive = new ()
        {
            text =
                "The moment I took the box, it was like a weight lifted off his shoulders. " +
                "Grinning from ear to ear, he kissed one of the pigeons on the beak, thanked me, and bounced off." +
                " Who knows what’s in this box, but I decided not to open it… not yet."
        };

        private static TextMeshProUGUI Mailman_Negative = new ()
        {
            text = "The moment I politely declined, the mailman’s friendly face twisted, and the flock of pigeons circling him lunged at me as one. " +
                   "Their sharp beaks stabbed like tiny spears into my skin. " +
                   "The last thing I remember was that cursed little box, tucked back into the mailman’s pocket."
        };

        #endregion Mailman
        
        #region Stray Cat

        private static TextMeshProUGUI StrayCat_Main = new ()
        {
            text = 
                "At the very edge of the road, perched on a pedestal as if placed there on purpose, sat a cat. " +
                "Thin, scruffy, obviously homeless. And it meowed so plaintively that my heart clenched into a knot."
        };

        private static TextMeshProUGUI StrayCat_Positive = new ()
        {
            text =
                "Since I had nothing to feed it, I decided not to tempt fate — for me or for the cat — and simply walked past. Sorry, little one."
        };

        private static TextMeshProUGUI StrayCat_Negative = new ()
        {
            text = "Even though I had nothing to feed it, I stepped closer to pet it. " +
                  "The cat purred immediately, rubbing against my hand — until, suddenly, my finger was in its teeth. " +
                  "My head spun for some reason, and the skin on my hand quickly reddened and bubbled with sores. " +
                  "\"Is this… a new kind of rabies?\" " +
                  "was all I managed to think before losing consciousness."
        };

        #endregion Stray Cat
        
                
        #region Traffic Officer

        private static TextMeshProUGUI TrafficOfficer_Main = new ()
        {
            text = 
                "The next second, a loud, piercing whistle nearly made me jump out of my skin." +
                "\n<color=red>Officer: “Stop! Stop right there, young man! It’s the right lane’s turn to go!”</color>" +
                "\nRaising his baton high, the officer froze in place like a tin soldier.\n"
        };

        private static TextMeshProUGUI TrafficOfficer_Positive = new ()
        {
            text =
                "As ridiculous as the situation seemed, I stopped and stood there for maybe half a minute. " +
                "Finally, the officer turned, waved his hand, and signaled for me to go. " +
                "Weird guy — but so serious about it that as I passed, I couldn’t help but give him a salute."
        };

        private static TextMeshProUGUI TrafficOfficer_Negative = new ()
        {
            text = "I’d never heard anything more absurd in my life, so I ignored the command and boldly stepped forward. " +
                   "To my shock, a truck materialized out of nowhere — speeding straight toward me. " +
                   "It didn’t even have time to brake before smearing me across its hood."
        };

        #endregion Stray Cat
    }
}