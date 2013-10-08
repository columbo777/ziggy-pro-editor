using ProUpgradeEditor.Common;
namespace ProUpgradeEditor.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox24;
            System.Windows.Forms.GroupBox groupBox15;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.GroupBox groupBox14;
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("File Name", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Size", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Date", System.Windows.Forms.HorizontalAlignment.Left);
            this.panel8 = new System.Windows.Forms.Panel();
            this.midiTrackEditorG5 = new EditorResources.Components.PEMidiTrackEditPanel();
            this.buttonSongToolSnapNotes = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonShortToG5Len = new System.Windows.Forms.Button();
            this.button136 = new System.Windows.Forms.Button();
            this.button134 = new System.Windows.Forms.Button();
            this.checkGenDiffCopyGuitarToBass = new System.Windows.Forms.CheckBox();
            this.button128 = new System.Windows.Forms.Button();
            this.button56 = new System.Windows.Forms.Button();
            this.button126 = new System.Windows.Forms.Button();
            this.button123 = new System.Windows.Forms.Button();
            this.buttonInitFromG5 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button77 = new System.Windows.Forms.Button();
            this.buttonInitTempo = new System.Windows.Forms.Button();
            this.button59 = new System.Windows.Forms.Button();
            this.checkBoxInitSelectedDifficultyOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxInitSelectedTrackOnly = new System.Windows.Forms.CheckBox();
            this.button31 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.checkBoxIncludeGuitar22 = new System.Windows.Forms.CheckBox();
            this.radioDifficultyExpert = new System.Windows.Forms.RadioButton();
            this.radioDifficultyHard = new System.Windows.Forms.RadioButton();
            this.radioDifficultyMedium = new System.Windows.Forms.RadioButton();
            this.radioDifficultyEasy = new System.Windows.Forms.RadioButton();
            this.panelTrackEditorPro = new System.Windows.Forms.Panel();
            this.midiTrackEditorPro = new EditorResources.Components.PEMidiTrackEditPanel();
            this.contextStripMidiTracks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileName5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripFileName6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCreateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabContainerMain = new System.Windows.Forms.TabControl();
            this.tabSongLibraryUtility = new System.Windows.Forms.TabPage();
            this.groupBox39 = new System.Windows.Forms.GroupBox();
            this.groupBox48 = new System.Windows.Forms.GroupBox();
            this.checkBoxSongLibSongListSortAscending = new System.Windows.Forms.CheckBox();
            this.radioSongLibSongListSortCompleted = new System.Windows.Forms.RadioButton();
            this.radioSongLibSongListSortID = new System.Windows.Forms.RadioButton();
            this.radioSongLibSongListSortName = new System.Windows.Forms.RadioButton();
            this.buttonSongLibListFilterReset = new System.Windows.Forms.Button();
            this.label55 = new System.Windows.Forms.Label();
            this.textBoxSongLibListFilter = new System.Windows.Forms.TextBox();
            this.listBoxSongLibrary = new System.Windows.Forms.ListBox();
            this.buttonCreateSongFromOpenMidi = new System.Windows.Forms.Button();
            this.button68 = new System.Windows.Forms.Button();
            this.button69 = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabSongLibSongProperties = new System.Windows.Forms.TabPage();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.buttonSongPropertiesViewMp3Preview = new System.Windows.Forms.Button();
            this.buttonFindMP3Offset = new System.Windows.Forms.Button();
            this.groupBox46 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoGenGuitarEasy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoGenGuitarMedium = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoGenGuitarHard = new System.Windows.Forms.CheckBox();
            this.groupBox45 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoGenBassEasy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoGenBassMedium = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoGenBassHard = new System.Windows.Forms.CheckBox();
            this.checkBoxSongPropertiesEnableMP3Playback = new System.Windows.Forms.CheckBox();
            this.trackBarMP3Volume = new System.Windows.Forms.TrackBar();
            this.checkBoxEnableMidiPlayback = new System.Windows.Forms.CheckBox();
            this.trackBarMidiVolume = new System.Windows.Forms.TrackBar();
            this.textBoxSongPropertiesMP3StartOffset = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.buttonSongPropertiesExploreMP3Location = new System.Windows.Forms.Button();
            this.buttonSongPropertiesChooseMP3Location = new System.Windows.Forms.Button();
            this.label53 = new System.Windows.Forms.Label();
            this.textBoxSongPropertiesMP3Location = new System.Windows.Forms.TextBox();
            this.buttonSongLibCopyPackageToUSB = new System.Windows.Forms.Button();
            this.groupBox35 = new System.Windows.Forms.GroupBox();
            this.checkBoxSongLibHasBass = new System.Windows.Forms.CheckBox();
            this.checkBoxSongLibHasGuitar = new System.Windows.Forms.CheckBox();
            this.checkBoxSongLibCopyGuitar = new System.Windows.Forms.CheckBox();
            this.checkBoxSongLibIsComplete = new System.Windows.Forms.CheckBox();
            this.checkBoxSongLibIsFinalized = new System.Windows.Forms.CheckBox();
            this.buttonSongLibViewPackageContents = new System.Windows.Forms.Button();
            this.buttonSongPropertiesCheckPackage = new System.Windows.Forms.Button();
            this.buttonRebuildPackage = new System.Windows.Forms.Button();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.buttonSongPropertiesMidiPause = new System.Windows.Forms.Button();
            this.buttonSongPropertiesMidiPlay = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button61 = new System.Windows.Forms.Button();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.textBox43 = new System.Windows.Forms.TextBox();
            this.textBox44 = new System.Windows.Forms.TextBox();
            this.textBox45 = new System.Windows.Forms.TextBox();
            this.textBox46 = new System.Windows.Forms.TextBox();
            this.textBox47 = new System.Windows.Forms.TextBox();
            this.textBox48 = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.textBox42 = new System.Windows.Forms.TextBox();
            this.textBox41 = new System.Windows.Forms.TextBox();
            this.textBox40 = new System.Windows.Forms.TextBox();
            this.textBox39 = new System.Windows.Forms.TextBox();
            this.textBox38 = new System.Windows.Forms.TextBox();
            this.textBox37 = new System.Windows.Forms.TextBox();
            this.textBoxCONShortName = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.textBoxCONSongID = new System.Windows.Forms.TextBox();
            this.comboProGDifficulty = new System.Windows.Forms.ComboBox();
            this.comboProBDifficulty = new System.Windows.Forms.ComboBox();
            this.button80 = new System.Windows.Forms.Button();
            this.button66 = new System.Windows.Forms.Button();
            this.button79 = new System.Windows.Forms.Button();
            this.button71 = new System.Windows.Forms.Button();
            this.button78 = new System.Windows.Forms.Button();
            this.button65 = new System.Windows.Forms.Button();
            this.buttonSongPropertiesSaveChanges = new System.Windows.Forms.Button();
            this.label37 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.textBoxSongLibConFile = new System.Windows.Forms.TextBox();
            this.textBoxSongLibProMidiFileName = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.textBoxSongLibG5MidiFileName = new System.Windows.Forms.TextBox();
            this.tabSongLibUtility = new System.Windows.Forms.TabPage();
            this.checkBoxBatchUtilExtractXMLPro = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchUtilExtractXMLG5 = new System.Windows.Forms.CheckBox();
            this.button94 = new System.Windows.Forms.Button();
            this.textBoxBatchUtilExtractXML = new System.Windows.Forms.TextBox();
            this.checkBoxCompressAllInDefaultCONFolder = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.buttonBatchOpenResult = new System.Windows.Forms.Button();
            this.checkBatchOpenWhenCompleted = new System.Windows.Forms.CheckBox();
            this.textBoxSongLibBatchResults = new System.Windows.Forms.TextBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.buttonBatchBuildTextEvents = new System.Windows.Forms.Button();
            this.checkBatchGenerateTrainersIfNone = new System.Windows.Forms.CheckBox();
            this.checkBatchCopyTextEvents = new System.Windows.Forms.CheckBox();
            this.buttonExecuteBatchCopyUSB = new System.Windows.Forms.Button();
            this.checkBoxBatchCopyUSB = new System.Windows.Forms.CheckBox();
            this.button106 = new System.Windows.Forms.Button();
            this.button105 = new System.Windows.Forms.Button();
            this.buttonExecuteBatchGuitarBassCopy = new System.Windows.Forms.Button();
            this.button103 = new System.Windows.Forms.Button();
            this.checkBoxBatchCheckCON = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchGenerateDifficulties = new System.Windows.Forms.CheckBox();
            this.buttonSongLibCancel = new System.Windows.Forms.Button();
            this.button89 = new System.Windows.Forms.Button();
            this.checkBoxSkipGenIfEasyNotes = new System.Windows.Forms.CheckBox();
            this.checkBoxSetBassToGuitarDifficulty = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchGuitarBassCopy = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchRebuildCON = new System.Windows.Forms.CheckBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.button93 = new System.Windows.Forms.Button();
            this.checkBoxBatchProcessFinalized = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchProcessIncomplete = new System.Windows.Forms.CheckBox();
            this.checkBoxMultiSelectionSongList = new System.Windows.Forms.CheckBox();
            this.checkBoxSmokeTest = new System.Windows.Forms.CheckBox();
            this.progressBarGenCompletedDifficulty = new System.Windows.Forms.ProgressBar();
            this.button90 = new System.Windows.Forms.Button();
            this.button73 = new System.Windows.Forms.Button();
            this.button118 = new System.Windows.Forms.Button();
            this.button74 = new System.Windows.Forms.Button();
            this.textBoxCopyAllG5MidiFolder = new System.Windows.Forms.TextBox();
            this.textBoxCopyAllProFolder = new System.Windows.Forms.TextBox();
            this.textBoxCompressAllZipFile = new System.Windows.Forms.TextBox();
            this.textBoxCopyAllCONFolder = new System.Windows.Forms.TextBox();
            this.button95 = new System.Windows.Forms.Button();
            this.buttonCopyAllConToUSB = new System.Windows.Forms.Button();
            this.button82 = new System.Windows.Forms.Button();
            this.button72 = new System.Windows.Forms.Button();
            this.button117 = new System.Windows.Forms.Button();
            this.button75 = new System.Windows.Forms.Button();
            this.tabSongLibSongUtility = new System.Windows.Forms.TabPage();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.groupBox49 = new System.Windows.Forms.GroupBox();
            this.buttonSongUtilFindInFileResultsOpenWindow = new System.Windows.Forms.Button();
            this.checkBoxSongUtilFindInFileResultsOpenCompleted = new System.Windows.Forms.CheckBox();
            this.textBoxSongUtilFindInFileResults = new System.Windows.Forms.TextBox();
            this.groupBoxSongUtilFindInFile = new System.Windows.Forms.GroupBox();
            this.textBoxSongUtilFindFolder = new System.Windows.Forms.TextBox();
            this.checkBoxSongUtilFindInProOnly = new System.Windows.Forms.CheckBox();
            this.label60 = new System.Windows.Forms.Label();
            this.buttonSongUtilFindFolder = new System.Windows.Forms.Button();
            this.buttonSongUtilFindInFileDistinctText = new System.Windows.Forms.Button();
            this.checkBoxSongUtilFindInFileMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.checkBoxSongUtilFindInFileMatchCountOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxSongUtilFindInFileFirstMatchOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxSongUtilFindInFileSelectedSongOnly = new System.Windows.Forms.CheckBox();
            this.textBoxSongUtilFindInFileChan = new System.Windows.Forms.TextBox();
            this.textBoxSongUtilFindInFileText = new System.Windows.Forms.TextBox();
            this.textBoxSongUtilFindInFileData1 = new System.Windows.Forms.TextBox();
            this.textBoxSongUtilFindInFileData2 = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.buttonSongUtilFindInFileSearch = new System.Windows.Forms.Button();
            this.label58 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.buttonSongUtilSearchFolderExplore = new System.Windows.Forms.Button();
            this.textBoxSongUtilSearchFolder = new System.Windows.Forms.TextBox();
            this.buttonSongUtilSearchForG5FromOpenPro = new System.Windows.Forms.Button();
            this.tabTrackEditor = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.button131 = new System.Windows.Forms.Button();
            this.textBoxTempoDenominator = new System.Windows.Forms.TextBox();
            this.textBoxTempoNumerator = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button130 = new System.Windows.Forms.Button();
            this.tabNoteEditor = new System.Windows.Forms.TabPage();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.comboBoxNoteEditorChordName = new System.Windows.Forms.ComboBox();
            this.label61 = new System.Windows.Forms.Label();
            this.checkChordNameEb = new System.Windows.Forms.CheckBox();
            this.checkChordNameD = new System.Windows.Forms.CheckBox();
            this.checkChordNameDb = new System.Windows.Forms.CheckBox();
            this.checkChordNameC = new System.Windows.Forms.CheckBox();
            this.checkChordNameG = new System.Windows.Forms.CheckBox();
            this.checkChordNameGb = new System.Windows.Forms.CheckBox();
            this.checkChordNameB = new System.Windows.Forms.CheckBox();
            this.checkChordNameBb = new System.Windows.Forms.CheckBox();
            this.checkChordNameA = new System.Windows.Forms.CheckBox();
            this.checkChordNameAb = new System.Windows.Forms.CheckBox();
            this.checkChordNameF = new System.Windows.Forms.CheckBox();
            this.checkChordNameE = new System.Windows.Forms.CheckBox();
            this.checkChordNameSlash = new System.Windows.Forms.CheckBox();
            this.checkChordNameHide = new System.Windows.Forms.CheckBox();
            this.groupBox42 = new System.Windows.Forms.GroupBox();
            this.button99 = new System.Windows.Forms.Button();
            this.button100 = new System.Windows.Forms.Button();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.radioNoteEditDifficultyExpert = new System.Windows.Forms.RadioButton();
            this.radioNoteEditDifficultyHard = new System.Windows.Forms.RadioButton();
            this.radioNoteEditDifficultyMedium = new System.Windows.Forms.RadioButton();
            this.radioNoteEditDifficultyEasy = new System.Windows.Forms.RadioButton();
            this.groupBox37 = new System.Windows.Forms.GroupBox();
            this.checkBoxClearIfNoFrets = new System.Windows.Forms.CheckBox();
            this.checkBoxPlayMidiStrum = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableClearTimer = new System.Windows.Forms.CheckBox();
            this.checkBoxChordStrum = new System.Windows.Forms.CheckBox();
            this.checkChordMode = new System.Windows.Forms.CheckBox();
            this.checkThreeNotePowerChord = new System.Windows.Forms.CheckBox();
            this.checkTwoNotePowerChord = new System.Windows.Forms.CheckBox();
            this.checkRealtimeNotes = new System.Windows.Forms.CheckBox();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.checkBoxAllowOverwriteChord = new System.Windows.Forms.CheckBox();
            this.checkBoxUseCurrentChord = new System.Windows.Forms.CheckBox();
            this.textBoxPlaceNoteFret = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button109 = new System.Windows.Forms.Button();
            this.button108 = new System.Windows.Forms.Button();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.button97 = new System.Windows.Forms.Button();
            this.button98 = new System.Windows.Forms.Button();
            this.groupBoxMidiInstrument = new System.Windows.Forms.GroupBox();
            this.labelMidiInputDeviceState = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.button33 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.button37 = new System.Windows.Forms.Button();
            this.button38 = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteSelectedNotes = new System.Windows.Forms.Button();
            this.buttonPlayHoldBoxMidi = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button55 = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.buttonReplaceStoredChordWithCopyPattern = new System.Windows.Forms.Button();
            this.button116 = new System.Windows.Forms.Button();
            this.checkBoxMatchAllFrets = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxSearchByNoteType = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchByNoteStrum = new System.Windows.Forms.CheckBox();
            this.button88 = new System.Windows.Forms.Button();
            this.button87 = new System.Windows.Forms.Button();
            this.listBoxStoredChords = new System.Windows.Forms.ListBox();
            this.button50 = new System.Windows.Forms.Button();
            this.button51 = new System.Windows.Forms.Button();
            this.button52 = new System.Windows.Forms.Button();
            this.checkBoxSearchByNoteFret = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.buttonSelectForward = new System.Windows.Forms.Button();
            this.buttonFixOverlappingNotes = new System.Windows.Forms.Button();
            this.buttonNoteUtilSelectAll = new System.Windows.Forms.Button();
            this.buttonUtilMethodSetToG5 = new System.Windows.Forms.Button();
            this.buttonUtilMethodFindNoteLenZero = new System.Windows.Forms.Button();
            this.buttonDownOctave = new System.Windows.Forms.Button();
            this.buttonUpOctave = new System.Windows.Forms.Button();
            this.buttonUtilMethodSnapToG5 = new System.Windows.Forms.Button();
            this.button132 = new System.Windows.Forms.Button();
            this.buttonUp12 = new System.Windows.Forms.Button();
            this.buttonDownString = new System.Windows.Forms.Button();
            this.button76 = new System.Windows.Forms.Button();
            this.button124 = new System.Windows.Forms.Button();
            this.button122 = new System.Windows.Forms.Button();
            this.buttonAddHammeron = new System.Windows.Forms.Button();
            this.button85 = new System.Windows.Forms.Button();
            this.buttonAddSlideHammeron = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button53 = new System.Windows.Forms.Button();
            this.buttonSelectBack = new System.Windows.Forms.Button();
            this.button54 = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBoxNoteEditorSelectedChordDownTick = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordUpTick = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordTickLength = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.checkIsHammeron = new System.Windows.Forms.CheckBox();
            this.checkIsTap = new System.Windows.Forms.CheckBox();
            this.checkIsX = new System.Windows.Forms.CheckBox();
            this.checkIsSlideReversed = new System.Windows.Forms.CheckBox();
            this.checkIsSlide = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoSelectNext = new System.Windows.Forms.CheckBox();
            this.checkBoxClearAfterNote = new System.Windows.Forms.CheckBox();
            this.checkIndentBString = new System.Windows.Forms.CheckBox();
            this.checkScrollToSelection = new System.Windows.Forms.CheckBox();
            this.checkKBQuickEdit = new System.Windows.Forms.CheckBox();
            this.checkKeepSelection = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBoxNoteEditorSelectedChordChannelBox0 = new System.Windows.Forms.TextBox();
            this.contextMenuStripChannels = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxNoteEditorSelectedChordChannelBox5 = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordChannelBox4 = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordChannelBox3 = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordChannelBox2 = new System.Windows.Forms.TextBox();
            this.textBoxNoteEditorSelectedChordChannelBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxMatchRemoveExisting = new System.Windows.Forms.CheckBox();
            this.buttonNoteEditorCopyPatternPresetUpdate = new System.Windows.Forms.Button();
            this.buttonNoteEditorCopyPatternPresetRemove = new System.Windows.Forms.Button();
            this.buttonNoteEditorCopyPatternPresetCreate = new System.Windows.Forms.Button();
            this.comboNoteEditorCopyPatternPreset = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.button120 = new System.Windows.Forms.Button();
            this.button121 = new System.Windows.Forms.Button();
            this.checkBoxMatchForwardOnly = new System.Windows.Forms.CheckBox();
            this.checkMatchBeat = new System.Windows.Forms.CheckBox();
            this.checkBoxKeepLengths = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchSpacing = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.checkBoxFirstMatchOnly = new System.Windows.Forms.CheckBox();
            this.button29 = new System.Windows.Forms.Button();
            this.checkBoxMatchLength6 = new System.Windows.Forms.CheckBox();
            this.checkBoxMatchLengths = new System.Windows.Forms.CheckBox();
            this.button28 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.groupBoxStrumMarkers = new System.Windows.Forms.GroupBox();
            this.buttonStrumNone = new System.Windows.Forms.Button();
            this.checkStrumLow = new System.Windows.Forms.CheckBox();
            this.checkStrumMid = new System.Windows.Forms.CheckBox();
            this.checkStrumHigh = new System.Windows.Forms.CheckBox();
            this.tabModifierEditor = new System.Windows.Forms.TabPage();
            this.groupBoxSingleStringTremelo = new System.Windows.Forms.GroupBox();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.button49 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.button47 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.button45 = new System.Windows.Forms.Button();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.button46 = new System.Windows.Forms.Button();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.groupBoxMultiStringTremelo = new System.Windows.Forms.GroupBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.button43 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.button41 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.button40 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.groupBoxArpeggio = new System.Windows.Forms.GroupBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.button26 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.button58 = new System.Windows.Forms.Button();
            this.textBox30 = new System.Windows.Forms.TextBox();
            this.textBox29 = new System.Windows.Forms.TextBox();
            this.button23 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.checkBoxCreateArpeggioHelperNotes = new System.Windows.Forms.CheckBox();
            this.groupBoxPowerup = new System.Windows.Forms.GroupBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.textBox27 = new System.Windows.Forms.TextBox();
            this.button18 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.groupBoxSolo = new System.Windows.Forms.GroupBox();
            this.listBoxSolos = new System.Windows.Forms.ListBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox26 = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.groupBox43 = new System.Windows.Forms.GroupBox();
            this.buttonRefresh108Events = new System.Windows.Forms.Button();
            this.checkBoxShow108 = new System.Windows.Forms.CheckBox();
            this.comboBox180 = new System.Windows.Forms.ListBox();
            this.groupBoxTextEvents = new System.Windows.Forms.GroupBox();
            this.buttonAddTextEvent = new System.Windows.Forms.Button();
            this.checkBoxShowTrainersInTextEvents = new System.Windows.Forms.CheckBox();
            this.listTextEvents = new System.Windows.Forms.ListBox();
            this.buttonRefreshTextEvents = new System.Windows.Forms.Button();
            this.buttonDeleteTextEvent = new System.Windows.Forms.Button();
            this.textBoxEventTick = new System.Windows.Forms.TextBox();
            this.textBoxEventText = new System.Windows.Forms.TextBox();
            this.buttonUpdateTextEvent = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBoxProBassTrainers = new System.Windows.Forms.GroupBox();
            this.checkTrainerLoopableProBass = new System.Windows.Forms.CheckBox();
            this.listProBassTrainers = new System.Windows.Forms.ListBox();
            this.buttonRefreshProBassTrainer = new System.Windows.Forms.Button();
            this.buttonRemoveProBassTrainer = new System.Windows.Forms.Button();
            this.buttonCreateProBassTrainer = new System.Windows.Forms.Button();
            this.labelProBassTrainerStatus = new System.Windows.Forms.Label();
            this.textBoxProBassTrainerBeginTick = new System.Windows.Forms.TextBox();
            this.textBoxProBassTrainerEndTick = new System.Windows.Forms.TextBox();
            this.buttonUpdateProBassTrainer = new System.Windows.Forms.Button();
            this.buttonCancelProBassTrainer = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.groupBoxProGuitarTrainers = new System.Windows.Forms.GroupBox();
            this.checkTrainerLoopableProGuitar = new System.Windows.Forms.CheckBox();
            this.listProGuitarTrainers = new System.Windows.Forms.ListBox();
            this.button135 = new System.Windows.Forms.Button();
            this.buttonRemoveProGuitarTrainer = new System.Windows.Forms.Button();
            this.buttonAddProGuitarTrainer = new System.Windows.Forms.Button();
            this.labelProGuitarTrainerStatus = new System.Windows.Forms.Label();
            this.textBoxProGuitarTrainerBeginTick = new System.Windows.Forms.TextBox();
            this.textBoxProGuitarTrainerEndTick = new System.Windows.Forms.TextBox();
            this.buttonUpdateProGuitarTrainer = new System.Windows.Forms.Button();
            this.buttonCancelProGuitarTrainer = new System.Windows.Forms.Button();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.tabPackageEditor = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.buttonPackageViewerSave = new System.Windows.Forms.Button();
            this.checkBoxPackageEditExtractDTAMidOnly = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button67 = new System.Windows.Forms.Button();
            this.textBoxPackageDTAText = new System.Windows.Forms.TextBox();
            this.treePackageContents = new System.Windows.Forms.TreeView();
            this.contextToolStripPackageEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripPackageEditorDeleteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPackageEditorOpenPackage = new System.Windows.Forms.Button();
            this.label41 = new System.Windows.Forms.Label();
            this.button60 = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.button81 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.textBox49 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button91 = new System.Windows.Forms.Button();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.buttonCheckAllInFolder = new System.Windows.Forms.Button();
            this.button92 = new System.Windows.Forms.Button();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabUSBDrive = new System.Windows.Forms.TabPage();
            this.buttonUSBCheckFile = new System.Windows.Forms.Button();
            this.listBoxUSBSongs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listUSBFileView = new System.Windows.Forms.ListView();
            this.treeUSBContents = new System.Windows.Forms.TreeView();
            this.progressUSBFiles = new System.Windows.Forms.ProgressBar();
            this.textBoxUSBFileName = new System.Windows.Forms.TextBox();
            this.buttonUSBAddFolder = new System.Windows.Forms.Button();
            this.progressUSBSongs = new System.Windows.Forms.ProgressBar();
            this.label47 = new System.Windows.Forms.Label();
            this.buttonUSBRestoreImage = new System.Windows.Forms.Button();
            this.buttonUSBCreateImage = new System.Windows.Forms.Button();
            this.textBoxUSBFolder = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.comboUSBList = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.buttonUSBRenameFile = new System.Windows.Forms.Button();
            this.buttonUSBViewPackage = new System.Windows.Forms.Button();
            this.buttonUSBSelectCompletedSongs = new System.Windows.Forms.Button();
            this.buttonUSBSelectAllSongs = new System.Windows.Forms.Button();
            this.buttonUSBAddFile = new System.Windows.Forms.Button();
            this.buttonUSBDeleteFile = new System.Windows.Forms.Button();
            this.buttonUSBSetDefaultFolder = new System.Windows.Forms.Button();
            this.buttonUSBRenameFolder = new System.Windows.Forms.Button();
            this.buttonUSBCreateFolder = new System.Windows.Forms.Button();
            this.buttonUSBRefresh = new System.Windows.Forms.Button();
            this.buttonUSBCopySelectedSongToUSB = new System.Windows.Forms.Button();
            this.buttonUSBExtractSelectedFiles = new System.Windows.Forms.Button();
            this.buttonUSBExtractFolder = new System.Windows.Forms.Button();
            this.buttonUSBDeleteSelected = new System.Windows.Forms.Button();
            this.button125 = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox38 = new System.Windows.Forms.GroupBox();
            this.button115 = new System.Windows.Forms.Button();
            this.button114 = new System.Windows.Forms.Button();
            this.button112 = new System.Windows.Forms.Button();
            this.button111 = new System.Windows.Forms.Button();
            this.comboBoxMidiInput = new System.Windows.Forms.ComboBox();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.button113 = new System.Windows.Forms.Button();
            this.button110 = new System.Windows.Forms.Button();
            this.comboMidiDevice = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button70 = new System.Windows.Forms.Button();
            this.comboMidiInstrument = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.checkView5Button = new System.Windows.Forms.CheckBox();
            this.checkBoxMidiInputStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableMidiInput = new System.Windows.Forms.CheckBox();
            this.textBoxNoteCloseDist = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textClearHoldBox = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBoxMidiScrollFreq = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.checkBoxLoadLastSongStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxMidiPlaybackScroll = new System.Windows.Forms.CheckBox();
            this.button57 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMinimumNoteWidth = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBoxScrollToSelectionOffset = new System.Windows.Forms.TextBox();
            this.checkBoxShowMidiChannelEdit = new System.Windows.Forms.CheckBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.textBoxDefaultCONFileLocation = new System.Windows.Forms.TextBox();
            this.textBoxDefaultMidi5FileLocation = new System.Windows.Forms.TextBox();
            this.textBoxDefaultMidiProFileLocation = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.button62 = new System.Windows.Forms.Button();
            this.checkUseDefaultFolders = new System.Windows.Forms.CheckBox();
            this.label32 = new System.Windows.Forms.Label();
            this.button64 = new System.Windows.Forms.Button();
            this.button63 = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxZoom = new System.Windows.Forms.TextBox();
            this.button27 = new System.Windows.Forms.Button();
            this.button84 = new System.Windows.Forms.Button();
            this.button83 = new System.Windows.Forms.Button();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.radioGridHalfNote = new System.Windows.Forms.RadioButton();
            this.radioGridWholeNote = new System.Windows.Forms.RadioButton();
            this.checkSnapToCloseG5 = new System.Windows.Forms.CheckBox();
            this.button119 = new System.Windows.Forms.Button();
            this.textBoxNoteSnapDistance = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBoxGridSnapDistance = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.checkBoxRenderMouseSnap = new System.Windows.Forms.CheckBox();
            this.checkBoxSnapToCloseNotes = new System.Windows.Forms.CheckBox();
            this.radioGrid128thNote = new System.Windows.Forms.RadioButton();
            this.checkBoxGridSnap = new System.Windows.Forms.CheckBox();
            this.radioGrid32Note = new System.Windows.Forms.RadioButton();
            this.checkViewNotesGrid5Button = new System.Windows.Forms.CheckBox();
            this.checkViewNotesGridPro = new System.Windows.Forms.CheckBox();
            this.radioGrid16Note = new System.Windows.Forms.RadioButton();
            this.radioGrid64thNote = new System.Windows.Forms.RadioButton();
            this.radioGrid8Note = new System.Windows.Forms.RadioButton();
            this.radioGridQuarterNote = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonCloseG6Track = new System.Windows.Forms.Button();
            this.buttonCloseG5Track = new System.Windows.Forms.Button();
            this.timerMidiPlayback = new System.Windows.Forms.Timer(this.components);
            this.panel5ButtonEditor = new System.Windows.Forms.Panel();
            this.trackEditorG5 = new ProUpgradeEditor.Common.TrackEditor();
            this.panelTrackEditorG5TitleBar = new System.Windows.Forms.Panel();
            this.buttonMinimizeG5 = new System.Windows.Forms.Button();
            this.buttonMaximizeG5 = new System.Windows.Forms.Button();
            this.labelStatusIconEditor5 = new System.Windows.Forms.Label();
            this.labelCurrentLoadedG5 = new System.Windows.Forms.Label();
            this.panelProEditor = new System.Windows.Forms.Panel();
            this.trackEditorG6 = new ProUpgradeEditor.Common.TrackEditor();
            this.panelTrackEditorG6TitleBar = new System.Windows.Forms.Panel();
            this.buttonMinimizeG6 = new System.Windows.Forms.Button();
            this.buttonMaximizeG6 = new System.Windows.Forms.Button();
            this.labelStatusIconEditor6 = new System.Windows.Forms.Label();
            this.labelCurrentLoadedG6 = new System.Windows.Forms.Label();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPro17ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCONPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigurationAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cONPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zipFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.midiExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeProMidiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guitarProToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xBoxUSBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupBox24 = new System.Windows.Forms.GroupBox();
            groupBox15 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox14 = new System.Windows.Forms.GroupBox();
            groupBox24.SuspendLayout();
            this.panel8.SuspendLayout();
            groupBox15.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox14.SuspendLayout();
            this.panelTrackEditorPro.SuspendLayout();
            this.contextStripMidiTracks.SuspendLayout();
            this.tabContainerMain.SuspendLayout();
            this.tabSongLibraryUtility.SuspendLayout();
            this.groupBox39.SuspendLayout();
            this.groupBox48.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabSongLibSongProperties.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox46.SuspendLayout();
            this.groupBox45.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMP3Volume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidiVolume)).BeginInit();
            this.groupBox35.SuspendLayout();
            this.groupBox28.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.tabSongLibUtility.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.tabSongLibSongUtility.SuspendLayout();
            this.groupBox47.SuspendLayout();
            this.groupBox49.SuspendLayout();
            this.groupBoxSongUtilFindInFile.SuspendLayout();
            this.tabTrackEditor.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox41.SuspendLayout();
            this.tabNoteEditor.SuspendLayout();
            this.groupBox44.SuspendLayout();
            this.groupBox42.SuspendLayout();
            this.groupBox40.SuspendLayout();
            this.groupBox37.SuspendLayout();
            this.groupBox36.SuspendLayout();
            this.groupBox34.SuspendLayout();
            this.groupBox29.SuspendLayout();
            this.groupBoxMidiInstrument.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.contextMenuStripChannels.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxStrumMarkers.SuspendLayout();
            this.tabModifierEditor.SuspendLayout();
            this.groupBoxSingleStringTremelo.SuspendLayout();
            this.groupBoxMultiStringTremelo.SuspendLayout();
            this.groupBoxArpeggio.SuspendLayout();
            this.groupBoxPowerup.SuspendLayout();
            this.groupBoxSolo.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.groupBox43.SuspendLayout();
            this.groupBoxTextEvents.SuspendLayout();
            this.groupBoxProBassTrainers.SuspendLayout();
            this.groupBoxProGuitarTrainers.SuspendLayout();
            this.tabPackageEditor.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.groupBox32.SuspendLayout();
            this.contextToolStripPackageEditor.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox31.SuspendLayout();
            this.groupBox30.SuspendLayout();
            this.tabUSBDrive.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox38.SuspendLayout();
            this.groupBox33.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.panel5ButtonEditor.SuspendLayout();
            this.panelTrackEditorG5TitleBar.SuspendLayout();
            this.panelProEditor.SuspendLayout();
            this.panelTrackEditorG6TitleBar.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox24
            // 
            groupBox24.Controls.Add(this.panel8);
            groupBox24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            groupBox24.Location = new System.Drawing.Point(8, 9);
            groupBox24.Name = "groupBox24";
            groupBox24.Size = new System.Drawing.Size(226, 410);
            groupBox24.TabIndex = 0;
            groupBox24.TabStop = false;
            groupBox24.Text = "5 Button Midi Tracks";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.midiTrackEditorG5);
            this.panel8.Location = new System.Drawing.Point(6, 19);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(214, 385);
            this.panel8.TabIndex = 32;
            // 
            // midiTrackEditorG5
            // 
            this.midiTrackEditorG5.AllowDrop = true;
            this.midiTrackEditorG5.BackColor = System.Drawing.Color.Transparent;
            this.midiTrackEditorG5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.midiTrackEditorG5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midiTrackEditorG5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.midiTrackEditorG5.IsPro = false;
            this.midiTrackEditorG5.Location = new System.Drawing.Point(0, 0);
            this.midiTrackEditorG5.Margin = new System.Windows.Forms.Padding(0);
            this.midiTrackEditorG5.Name = "midiTrackEditorG5";
            this.midiTrackEditorG5.SelectedDifficulty = ProUpgradeEditor.Common.GuitarDifficulty.Expert;
            this.midiTrackEditorG5.Size = new System.Drawing.Size(214, 385);
            this.midiTrackEditorG5.TabIndex = 0;
            this.midiTrackEditorG5.TrackAdded += new EditorResources.Components.TrackEditPanelEventHandler(this.midiTrackEditorG5_TrackAdded);
            this.midiTrackEditorG5.TrackClicked += new EditorResources.Components.TrackEditPanelEventHandler(this.midiTrackEditorG5_TrackClicked);
            // 
            // groupBox15
            // 
            groupBox15.Controls.Add(this.buttonSongToolSnapNotes);
            groupBox15.Controls.Add(this.buttonShortToG5Len);
            groupBox15.Controls.Add(this.button136);
            groupBox15.Controls.Add(this.button134);
            groupBox15.Controls.Add(this.checkGenDiffCopyGuitarToBass);
            groupBox15.Controls.Add(this.button128);
            groupBox15.Controls.Add(this.button56);
            groupBox15.Controls.Add(this.button126);
            groupBox15.Controls.Add(this.button123);
            groupBox15.Controls.Add(this.buttonInitFromG5);
            groupBox15.Controls.Add(this.button10);
            groupBox15.Controls.Add(this.button77);
            groupBox15.Controls.Add(this.buttonInitTempo);
            groupBox15.Controls.Add(this.button59);
            groupBox15.Controls.Add(this.checkBoxInitSelectedDifficultyOnly);
            groupBox15.Controls.Add(this.checkBoxInitSelectedTrackOnly);
            groupBox15.Controls.Add(this.button31);
            groupBox15.Controls.Add(this.button44);
            groupBox15.Controls.Add(this.checkBoxIncludeGuitar22);
            groupBox15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            groupBox15.Location = new System.Drawing.Point(487, 9);
            groupBox15.Name = "groupBox15";
            groupBox15.Size = new System.Drawing.Size(338, 410);
            groupBox15.TabIndex = 2;
            groupBox15.TabStop = false;
            groupBox15.Text = "Tools";
            groupBox15.Enter += new System.EventHandler(this.groupBox15_Enter);
            // 
            // buttonSongToolSnapNotes
            // 
            this.buttonSongToolSnapNotes.BackColor = System.Drawing.Color.Transparent;
            this.buttonSongToolSnapNotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSongToolSnapNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongToolSnapNotes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSongToolSnapNotes.ImageIndex = 14;
            this.buttonSongToolSnapNotes.ImageList = this.imageList1;
            this.buttonSongToolSnapNotes.Location = new System.Drawing.Point(16, 235);
            this.buttonSongToolSnapNotes.Name = "buttonSongToolSnapNotes";
            this.buttonSongToolSnapNotes.Size = new System.Drawing.Size(134, 24);
            this.buttonSongToolSnapNotes.TabIndex = 16;
            this.buttonSongToolSnapNotes.Text = "Snap Notes";
            this.buttonSongToolSnapNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSongToolSnapNotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonSongToolSnapNotes, "Snap notes to the guitar 5 track");
            this.buttonSongToolSnapNotes.UseVisualStyleBackColor = true;
            this.buttonSongToolSnapNotes.Click += new System.EventHandler(this.buttonSongToolSnapNotes_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_explore.png");
            this.imageList1.Images.SetKeyName(1, "XPRecycle.gif");
            this.imageList1.Images.SetKeyName(2, "doc_music.png");
            this.imageList1.Images.SetKeyName(3, "folder_classic_type_music.png");
            this.imageList1.Images.SetKeyName(4, "folder_modernist_type_music.png");
            this.imageList1.Images.SetKeyName(5, "music (1).png");
            this.imageList1.Images.SetKeyName(6, "music.png");
            this.imageList1.Images.SetKeyName(7, "music_beam.png");
            this.imageList1.Images.SetKeyName(8, "music_green.png");
            this.imageList1.Images.SetKeyName(9, "music_grey.png");
            this.imageList1.Images.SetKeyName(10, "music-beam-16.png");
            this.imageList1.Images.SetKeyName(11, "music--exclamation.png");
            this.imageList1.Images.SetKeyName(12, "music--minus.png");
            this.imageList1.Images.SetKeyName(13, "music--pencil.png");
            this.imageList1.Images.SetKeyName(14, "music--plus.png");
            this.imageList1.Images.SetKeyName(15, "close.png");
            this.imageList1.Images.SetKeyName(16, "Task.gif");
            this.imageList1.Images.SetKeyName(17, "XPLens.gif");
            this.imageList1.Images.SetKeyName(18, "USBBlueArrow.png");
            this.imageList1.Images.SetKeyName(19, "USBExcl.png");
            this.imageList1.Images.SetKeyName(20, "USBFlash.png");
            this.imageList1.Images.SetKeyName(21, "USBGreenPlus.png");
            this.imageList1.Images.SetKeyName(22, "USBLogo.png");
            this.imageList1.Images.SetKeyName(23, "USBPencil.png");
            this.imageList1.Images.SetKeyName(24, "USBRedMinus.png");
            this.imageList1.Images.SetKeyName(25, "Partition.png");
            this.imageList1.Images.SetKeyName(26, "XPDrive.gif");
            this.imageList1.Images.SetKeyName(27, "piano_exclamation.png");
            this.imageList1.Images.SetKeyName(28, "edit-select-all.png");
            this.imageList1.Images.SetKeyName(29, "old-edit-find.png");
            this.imageList1.Images.SetKeyName(30, "disk.png");
            this.imageList1.Images.SetKeyName(31, "redx.png");
            this.imageList1.Images.SetKeyName(32, "remove_minus_sign_small.png");
            this.imageList1.Images.SetKeyName(33, "Plus__Orange.png");
            this.imageList1.Images.SetKeyName(34, "plus_16.png");
            this.imageList1.Images.SetKeyName(35, "Add.png");
            this.imageList1.Images.SetKeyName(36, "gtk-refresh.png");
            this.imageList1.Images.SetKeyName(37, "rebuild.png");
            this.imageList1.Images.SetKeyName(38, "blue-document-music-playlist.png");
            this.imageList1.Images.SetKeyName(39, "blue-folder-open-document-music.png");
            this.imageList1.Images.SetKeyName(40, "blue-folder-open-document-music-playlist.png");
            this.imageList1.Images.SetKeyName(41, "folder-open-document-music.png");
            this.imageList1.Images.SetKeyName(42, "document-music-playlist.png");
            this.imageList1.Images.SetKeyName(43, "music--arrow.png");
            this.imageList1.Images.SetKeyName(44, "1341680384_folder_midi.png");
            this.imageList1.Images.SetKeyName(45, "1341680345_audio-midi.png");
            this.imageList1.Images.SetKeyName(46, "node-tree.png");
            this.imageList1.Images.SetKeyName(47, "node-insert-next.png");
            this.imageList1.Images.SetKeyName(48, "node-select-all.png");
            this.imageList1.Images.SetKeyName(49, "external.png");
            this.imageList1.Images.SetKeyName(50, "OpenFolder.gif");
            this.imageList1.Images.SetKeyName(51, "XPFolder.gif");
            this.imageList1.Images.SetKeyName(52, "file_extension_ace.png");
            this.imageList1.Images.SetKeyName(53, "file_extension_m4b.png");
            this.imageList1.Images.SetKeyName(54, "file_extension_aif.png");
            this.imageList1.Images.SetKeyName(55, "file_extension_m4v.png");
            this.imageList1.Images.SetKeyName(56, "file_extension_amr.png");
            this.imageList1.Images.SetKeyName(57, "file_extension_rtf.png");
            this.imageList1.Images.SetKeyName(58, "file_extension_txt.png");
            this.imageList1.Images.SetKeyName(59, "file_extension_mid.png");
            this.imageList1.Images.SetKeyName(60, "ark_extract.png");
            this.imageList1.Images.SetKeyName(61, "EditorIcon.ico");
            this.imageList1.Images.SetKeyName(62, "page_white_zip.png");
            this.imageList1.Images.SetKeyName(63, "gear__exclamation.png");
            this.imageList1.Images.SetKeyName(64, "gear__minus.png");
            this.imageList1.Images.SetKeyName(65, "gear--pencil.png");
            this.imageList1.Images.SetKeyName(66, "gear__plus.png");
            this.imageList1.Images.SetKeyName(67, "gear__arrow.png");
            this.imageList1.Images.SetKeyName(68, "gear yellow.png");
            this.imageList1.Images.SetKeyName(69, "gear_16.png");
            this.imageList1.Images.SetKeyName(70, "special-offer (1).png");
            this.imageList1.Images.SetKeyName(71, "special-offer.png");
            this.imageList1.Images.SetKeyName(72, "file_extension_dll.png");
            this.imageList1.Images.SetKeyName(73, "file_extension_chm.png");
            this.imageList1.Images.SetKeyName(74, "clipboard-sign-out.png");
            this.imageList1.Images.SetKeyName(75, "media_controls_stop_small.png");
            this.imageList1.Images.SetKeyName(76, "media_controls_pause_small.png");
            this.imageList1.Images.SetKeyName(77, "media_controls_play_small.png");
            this.imageList1.Images.SetKeyName(78, "copy.png");
            this.imageList1.Images.SetKeyName(79, "textfield_rename.png");
            this.imageList1.Images.SetKeyName(80, "resultset_next.png");
            this.imageList1.Images.SetKeyName(81, "resultset_previous.png");
            this.imageList1.Images.SetKeyName(82, "clear-left.png");
            this.imageList1.Images.SetKeyName(83, "magnifier-zoom-in.png");
            this.imageList1.Images.SetKeyName(84, "magnifier-zoom-out.png");
            this.imageList1.Images.SetKeyName(85, "gtk-zoom-in.png");
            this.imageList1.Images.SetKeyName(86, "gtk-zoom-out.png");
            this.imageList1.Images.SetKeyName(87, "magnifier_zoom_in.png");
            this.imageList1.Images.SetKeyName(88, "plug--arrow.png");
            this.imageList1.Images.SetKeyName(89, "plug_plus.png");
            this.imageList1.Images.SetKeyName(90, "plug_pencil.png");
            this.imageList1.Images.SetKeyName(91, "plug--minus.png");
            this.imageList1.Images.SetKeyName(92, "plug--exclamation.png");
            this.imageList1.Images.SetKeyName(93, "slideIcon.gif");
            // 
            // buttonShortToG5Len
            // 
            this.buttonShortToG5Len.BackColor = System.Drawing.Color.Transparent;
            this.buttonShortToG5Len.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonShortToG5Len.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonShortToG5Len.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShortToG5Len.ImageIndex = 14;
            this.buttonShortToG5Len.ImageList = this.imageList1;
            this.buttonShortToG5Len.Location = new System.Drawing.Point(162, 211);
            this.buttonShortToG5Len.Name = "buttonShortToG5Len";
            this.buttonShortToG5Len.Size = new System.Drawing.Size(133, 24);
            this.buttonShortToG5Len.TabIndex = 15;
            this.buttonShortToG5Len.Text = "Short To G5 Len";
            this.buttonShortToG5Len.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShortToG5Len.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonShortToG5Len, "Set any invalid length notes to the same length as in the Guitar5 midi");
            this.buttonShortToG5Len.UseVisualStyleBackColor = true;
            this.buttonShortToG5Len.Click += new System.EventHandler(this.buttonShortToG5Len_Click);
            // 
            // button136
            // 
            this.button136.BackColor = System.Drawing.Color.Transparent;
            this.button136.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button136.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button136.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button136.ImageIndex = 14;
            this.button136.ImageList = this.imageList1;
            this.button136.Location = new System.Drawing.Point(162, 187);
            this.button136.Name = "button136";
            this.button136.Size = new System.Drawing.Size(133, 24);
            this.button136.TabIndex = 14;
            this.button136.Text = "Copy Text Events";
            this.button136.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button136.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button136, "Copy the Text Events from the 5 button midi file");
            this.button136.UseVisualStyleBackColor = true;
            this.button136.Click += new System.EventHandler(this.button136_Click);
            // 
            // button134
            // 
            this.button134.BackColor = System.Drawing.Color.Transparent;
            this.button134.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button134.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button134.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button134.ImageIndex = 14;
            this.button134.ImageList = this.imageList1;
            this.button134.Location = new System.Drawing.Point(162, 163);
            this.button134.Name = "button134";
            this.button134.Size = new System.Drawing.Size(133, 24);
            this.button134.TabIndex = 13;
            this.button134.Text = "Shorten Notes";
            this.button134.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button134.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button134, "Shorten All notes by one");
            this.button134.UseVisualStyleBackColor = true;
            this.button134.Click += new System.EventHandler(this.button134_Click);
            // 
            // checkGenDiffCopyGuitarToBass
            // 
            this.checkGenDiffCopyGuitarToBass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkGenDiffCopyGuitarToBass.Location = new System.Drawing.Point(162, 20);
            this.checkGenDiffCopyGuitarToBass.Name = "checkGenDiffCopyGuitarToBass";
            this.checkGenDiffCopyGuitarToBass.Size = new System.Drawing.Size(152, 22);
            this.checkGenDiffCopyGuitarToBass.TabIndex = 5;
            this.checkGenDiffCopyGuitarToBass.Text = "Copy Guitar To Bass";
            this.checkGenDiffCopyGuitarToBass.UseCompatibleTextRendering = true;
            this.checkGenDiffCopyGuitarToBass.UseVisualStyleBackColor = true;
            // 
            // button128
            // 
            this.button128.BackColor = System.Drawing.Color.Transparent;
            this.button128.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button128.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button128.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button128.ImageIndex = 14;
            this.button128.ImageList = this.imageList1;
            this.button128.Location = new System.Drawing.Point(162, 115);
            this.button128.Name = "button128";
            this.button128.Size = new System.Drawing.Size(133, 24);
            this.button128.TabIndex = 12;
            this.button128.Text = "Create Bass Track";
            this.button128.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button128.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button128, "Create a bass guitar track from the guitar track");
            this.button128.UseVisualStyleBackColor = true;
            this.button128.Click += new System.EventHandler(this.button128_Click);
            // 
            // button56
            // 
            this.button56.BackColor = System.Drawing.Color.Transparent;
            this.button56.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button56.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button56.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button56.ImageIndex = 67;
            this.button56.ImageList = this.imageList1;
            this.button56.Location = new System.Drawing.Point(16, 67);
            this.button56.Name = "button56";
            this.button56.Size = new System.Drawing.Size(134, 24);
            this.button56.TabIndex = 4;
            this.button56.Text = "Gen Difficulties";
            this.button56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button56.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button56, "Generate Difficulties");
            this.button56.UseVisualStyleBackColor = true;
            this.button56.Click += new System.EventHandler(this.button56_Click);
            // 
            // button126
            // 
            this.button126.BackColor = System.Drawing.Color.Transparent;
            this.button126.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button126.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button126.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button126.ImageIndex = 71;
            this.button126.ImageList = this.imageList1;
            this.button126.Location = new System.Drawing.Point(16, 163);
            this.button126.Name = "button126";
            this.button126.Size = new System.Drawing.Size(134, 24);
            this.button126.TabIndex = 11;
            this.button126.Text = "Copy Powerups";
            this.button126.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button126.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button126, "Copy Solos");
            this.button126.UseVisualStyleBackColor = true;
            this.button126.Click += new System.EventHandler(this.button126_Click);
            // 
            // button123
            // 
            this.button123.BackColor = System.Drawing.Color.Transparent;
            this.button123.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button123.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button123.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button123.ImageIndex = 14;
            this.button123.ImageList = this.imageList1;
            this.button123.Location = new System.Drawing.Point(162, 91);
            this.button123.Name = "button123";
            this.button123.Size = new System.Drawing.Size(133, 24);
            this.button123.TabIndex = 10;
            this.button123.Text = "Create 22 Fret";
            this.button123.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button123.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button123, "Create 22 fret track copies of the existing tracks");
            this.button123.UseVisualStyleBackColor = true;
            this.button123.Click += new System.EventHandler(this.button123_Click);
            // 
            // buttonInitFromG5
            // 
            this.buttonInitFromG5.BackColor = System.Drawing.Color.Transparent;
            this.buttonInitFromG5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonInitFromG5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonInitFromG5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInitFromG5.ImageIndex = 66;
            this.buttonInitFromG5.ImageList = this.imageList1;
            this.buttonInitFromG5.Location = new System.Drawing.Point(162, 67);
            this.buttonInitFromG5.Name = "buttonInitFromG5";
            this.buttonInitFromG5.Size = new System.Drawing.Size(133, 24);
            this.buttonInitFromG5.TabIndex = 0;
            this.buttonInitFromG5.Text = "Init From 5 Button";
            this.buttonInitFromG5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInitFromG5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonInitFromG5.UseVisualStyleBackColor = true;
            this.buttonInitFromG5.Click += new System.EventHandler(this.buttonInitFromG5_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Transparent;
            this.button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.ImageIndex = 65;
            this.button10.ImageList = this.imageList1;
            this.button10.Location = new System.Drawing.Point(16, 91);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(134, 24);
            this.button10.TabIndex = 4;
            this.button10.Text = "Set 108 Events";
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.buttonSet108Events);
            // 
            // button77
            // 
            this.button77.BackColor = System.Drawing.Color.Transparent;
            this.button77.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button77.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button77.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button77.ImageIndex = 11;
            this.button77.ImageList = this.imageList1;
            this.button77.Location = new System.Drawing.Point(16, 211);
            this.button77.Name = "button77";
            this.button77.Size = new System.Drawing.Size(134, 24);
            this.button77.TabIndex = 9;
            this.button77.Text = "Replace Bass";
            this.button77.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button77.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button77, "Over-write the bass track with the guitar track");
            this.button77.UseVisualStyleBackColor = true;
            this.button77.Click += new System.EventHandler(this.button77_Click);
            // 
            // buttonInitTempo
            // 
            this.buttonInitTempo.BackColor = System.Drawing.Color.Transparent;
            this.buttonInitTempo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonInitTempo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonInitTempo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInitTempo.ImageIndex = 14;
            this.buttonInitTempo.ImageList = this.imageList1;
            this.buttonInitTempo.Location = new System.Drawing.Point(162, 139);
            this.buttonInitTempo.Name = "buttonInitTempo";
            this.buttonInitTempo.Size = new System.Drawing.Size(133, 24);
            this.buttonInitTempo.TabIndex = 11;
            this.buttonInitTempo.Text = "Copy Tempo";
            this.buttonInitTempo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInitTempo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonInitTempo, "Copy the tempo track from the 5 button midi file");
            this.buttonInitTempo.UseVisualStyleBackColor = true;
            this.buttonInitTempo.Click += new System.EventHandler(this.buttonInitTempo_Click);
            // 
            // button59
            // 
            this.button59.BackColor = System.Drawing.Color.Transparent;
            this.button59.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button59.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button59.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button59.ImageIndex = 66;
            this.button59.ImageList = this.imageList1;
            this.button59.Location = new System.Drawing.Point(16, 187);
            this.button59.Name = "button59";
            this.button59.Size = new System.Drawing.Size(134, 24);
            this.button59.TabIndex = 8;
            this.button59.Text = "Copy Difficulties";
            this.button59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button59.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button59, "Copy notes from current difficulty across all lower difficulties");
            this.button59.UseVisualStyleBackColor = true;
            this.button59.Click += new System.EventHandler(this.button59_Click);
            // 
            // checkBoxInitSelectedDifficultyOnly
            // 
            this.checkBoxInitSelectedDifficultyOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxInitSelectedDifficultyOnly.Location = new System.Drawing.Point(162, 39);
            this.checkBoxInitSelectedDifficultyOnly.Name = "checkBoxInitSelectedDifficultyOnly";
            this.checkBoxInitSelectedDifficultyOnly.Size = new System.Drawing.Size(152, 22);
            this.checkBoxInitSelectedDifficultyOnly.TabIndex = 3;
            this.checkBoxInitSelectedDifficultyOnly.Text = "Selected Difficulty Only";
            this.toolTip1.SetToolTip(this.checkBoxInitSelectedDifficultyOnly, "Initialize only the selected difficulty");
            this.checkBoxInitSelectedDifficultyOnly.UseCompatibleTextRendering = true;
            this.checkBoxInitSelectedDifficultyOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxInitSelectedTrackOnly
            // 
            this.checkBoxInitSelectedTrackOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxInitSelectedTrackOnly.Location = new System.Drawing.Point(16, 41);
            this.checkBoxInitSelectedTrackOnly.Name = "checkBoxInitSelectedTrackOnly";
            this.checkBoxInitSelectedTrackOnly.Size = new System.Drawing.Size(140, 18);
            this.checkBoxInitSelectedTrackOnly.TabIndex = 2;
            this.checkBoxInitSelectedTrackOnly.Text = "Selected Track Only";
            this.toolTip1.SetToolTip(this.checkBoxInitSelectedTrackOnly, "Initialize only the selected track");
            this.checkBoxInitSelectedTrackOnly.UseCompatibleTextRendering = true;
            this.checkBoxInitSelectedTrackOnly.UseVisualStyleBackColor = true;
            // 
            // button31
            // 
            this.button31.BackColor = System.Drawing.Color.Transparent;
            this.button31.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button31.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button31.ImageIndex = 66;
            this.button31.ImageList = this.imageList1;
            this.button31.Location = new System.Drawing.Point(16, 115);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(134, 24);
            this.button31.TabIndex = 6;
            this.button31.Text = "Copy B.R.E.";
            this.button31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button31.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button31, "Copy Big Rock Ending");
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button44
            // 
            this.button44.BackColor = System.Drawing.Color.Transparent;
            this.button44.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button44.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button44.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button44.ImageIndex = 14;
            this.button44.ImageList = this.imageList1;
            this.button44.Location = new System.Drawing.Point(16, 139);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(134, 24);
            this.button44.TabIndex = 7;
            this.button44.Text = "Copy Solos";
            this.button44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button44.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button44, "Copy Solos");
            this.button44.UseVisualStyleBackColor = true;
            this.button44.Click += new System.EventHandler(this.button44_Click);
            // 
            // checkBoxIncludeGuitar22
            // 
            this.checkBoxIncludeGuitar22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxIncludeGuitar22.Location = new System.Drawing.Point(16, 22);
            this.checkBoxIncludeGuitar22.Name = "checkBoxIncludeGuitar22";
            this.checkBoxIncludeGuitar22.Size = new System.Drawing.Size(140, 18);
            this.checkBoxIncludeGuitar22.TabIndex = 1;
            this.checkBoxIncludeGuitar22.Text = "Include 22 fret";
            this.toolTip1.SetToolTip(this.checkBoxIncludeGuitar22, "Create the 22 fret tracks");
            this.checkBoxIncludeGuitar22.UseCompatibleTextRendering = true;
            this.checkBoxIncludeGuitar22.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.radioDifficultyExpert);
            groupBox3.Controls.Add(this.radioDifficultyHard);
            groupBox3.Controls.Add(this.radioDifficultyMedium);
            groupBox3.Controls.Add(this.radioDifficultyEasy);
            groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            groupBox3.Location = new System.Drawing.Point(831, 9);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(122, 102);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Edit Difficulty";
            // 
            // radioDifficultyExpert
            // 
            this.radioDifficultyExpert.Checked = true;
            this.radioDifficultyExpert.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioDifficultyExpert.Location = new System.Drawing.Point(9, 76);
            this.radioDifficultyExpert.Name = "radioDifficultyExpert";
            this.radioDifficultyExpert.Size = new System.Drawing.Size(87, 18);
            this.radioDifficultyExpert.TabIndex = 3;
            this.radioDifficultyExpert.TabStop = true;
            this.radioDifficultyExpert.Text = "Expert";
            this.radioDifficultyExpert.UseCompatibleTextRendering = true;
            this.radioDifficultyExpert.UseVisualStyleBackColor = true;
            this.radioDifficultyExpert.Click += new System.EventHandler(this.radioDifficultyExpert_Click);
            // 
            // radioDifficultyHard
            // 
            this.radioDifficultyHard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioDifficultyHard.Location = new System.Drawing.Point(9, 56);
            this.radioDifficultyHard.Name = "radioDifficultyHard";
            this.radioDifficultyHard.Size = new System.Drawing.Size(87, 18);
            this.radioDifficultyHard.TabIndex = 2;
            this.radioDifficultyHard.Text = "Hard";
            this.radioDifficultyHard.UseCompatibleTextRendering = true;
            this.radioDifficultyHard.UseVisualStyleBackColor = true;
            this.radioDifficultyHard.Click += new System.EventHandler(this.radioDifficultyHard_Click);
            // 
            // radioDifficultyMedium
            // 
            this.radioDifficultyMedium.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioDifficultyMedium.Location = new System.Drawing.Point(9, 36);
            this.radioDifficultyMedium.Name = "radioDifficultyMedium";
            this.radioDifficultyMedium.Size = new System.Drawing.Size(87, 18);
            this.radioDifficultyMedium.TabIndex = 1;
            this.radioDifficultyMedium.Text = "Medium";
            this.radioDifficultyMedium.UseCompatibleTextRendering = true;
            this.radioDifficultyMedium.UseVisualStyleBackColor = true;
            this.radioDifficultyMedium.Click += new System.EventHandler(this.radioDifficultyMedium_Click);
            // 
            // radioDifficultyEasy
            // 
            this.radioDifficultyEasy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioDifficultyEasy.Location = new System.Drawing.Point(9, 16);
            this.radioDifficultyEasy.Name = "radioDifficultyEasy";
            this.radioDifficultyEasy.Size = new System.Drawing.Size(87, 18);
            this.radioDifficultyEasy.TabIndex = 0;
            this.radioDifficultyEasy.Text = "Easy";
            this.radioDifficultyEasy.UseCompatibleTextRendering = true;
            this.radioDifficultyEasy.UseVisualStyleBackColor = true;
            this.radioDifficultyEasy.Click += new System.EventHandler(this.radioDifficultyEasy_Click);
            // 
            // groupBox14
            // 
            groupBox14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            groupBox14.Controls.Add(this.panelTrackEditorPro);
            groupBox14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            groupBox14.Location = new System.Drawing.Point(234, 9);
            groupBox14.Name = "groupBox14";
            groupBox14.Size = new System.Drawing.Size(247, 410);
            groupBox14.TabIndex = 1;
            groupBox14.TabStop = false;
            groupBox14.Text = "Pro Midi Tracks";
            // 
            // panelTrackEditorPro
            // 
            this.panelTrackEditorPro.Controls.Add(this.midiTrackEditorPro);
            this.panelTrackEditorPro.Location = new System.Drawing.Point(6, 19);
            this.panelTrackEditorPro.Name = "panelTrackEditorPro";
            this.panelTrackEditorPro.Size = new System.Drawing.Size(235, 385);
            this.panelTrackEditorPro.TabIndex = 30;
            // 
            // midiTrackEditorPro
            // 
            this.midiTrackEditorPro.AllowDrop = true;
            this.midiTrackEditorPro.BackColor = System.Drawing.Color.Transparent;
            this.midiTrackEditorPro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.midiTrackEditorPro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midiTrackEditorPro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.midiTrackEditorPro.IsPro = true;
            this.midiTrackEditorPro.Location = new System.Drawing.Point(0, 0);
            this.midiTrackEditorPro.Margin = new System.Windows.Forms.Padding(0);
            this.midiTrackEditorPro.Name = "midiTrackEditorPro";
            this.midiTrackEditorPro.SelectedDifficulty = ProUpgradeEditor.Common.GuitarDifficulty.Expert;
            this.midiTrackEditorPro.Size = new System.Drawing.Size(235, 385);
            this.midiTrackEditorPro.TabIndex = 0;
            this.midiTrackEditorPro.TrackAdded += new EditorResources.Components.TrackEditPanelEventHandler(this.midiTrackEditorPro_TrackAdded);
            this.midiTrackEditorPro.TrackClicked += new EditorResources.Components.TrackEditPanelEventHandler(this.midiTrackEditorPro_TrackClicked);
            this.midiTrackEditorPro.Load += new System.EventHandler(this.midiTrackEditorPro_Load);
            // 
            // contextStripMidiTracks
            // 
            this.contextStripMidiTracks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.contextStripMidiTracks.Name = "contextStripMidiTracks";
            this.contextStripMidiTracks.Size = new System.Drawing.Size(108, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem1.Text = "Copy";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem2.Text = "Paste";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Enabled = false;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem3.Text = "Delete";
            // 
            // toolStripFileName5
            // 
            this.toolStripFileName5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFileName5.Name = "toolStripFileName5";
            this.toolStripFileName5.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // toolStripFileName6
            // 
            this.toolStripFileName6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFileName6.Name = "toolStripFileName6";
            this.toolStripFileName6.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel2.Text = "-";
            // 
            // toolStripCreateStatus
            // 
            this.toolStripCreateStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripCreateStatus.Name = "toolStripCreateStatus";
            this.toolStripCreateStatus.Size = new System.Drawing.Size(25, 17);
            this.toolStripCreateStatus.Text = "Idle";
            // 
            // tabContainerMain
            // 
            this.tabContainerMain.AllowDrop = true;
            this.tabContainerMain.Controls.Add(this.tabSongLibraryUtility);
            this.tabContainerMain.Controls.Add(this.tabTrackEditor);
            this.tabContainerMain.Controls.Add(this.tabNoteEditor);
            this.tabContainerMain.Controls.Add(this.tabModifierEditor);
            this.tabContainerMain.Controls.Add(this.tabPageEvents);
            this.tabContainerMain.Controls.Add(this.tabPackageEditor);
            this.tabContainerMain.Controls.Add(this.tabUSBDrive);
            this.tabContainerMain.Controls.Add(this.tabSettings);
            this.tabContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainerMain.ImageList = this.imageList1;
            this.tabContainerMain.Location = new System.Drawing.Point(0, 302);
            this.tabContainerMain.Name = "tabContainerMain";
            this.tabContainerMain.SelectedIndex = 0;
            this.tabContainerMain.Size = new System.Drawing.Size(1103, 550);
            this.tabContainerMain.TabIndex = 0;
            // 
            // tabSongLibraryUtility
            // 
            this.tabSongLibraryUtility.AutoScroll = true;
            this.tabSongLibraryUtility.AutoScrollMinSize = new System.Drawing.Size(1010, 522);
            this.tabSongLibraryUtility.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
            this.tabSongLibraryUtility.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabSongLibraryUtility.Controls.Add(this.groupBox39);
            this.tabSongLibraryUtility.Controls.Add(this.tabControl2);
            this.tabSongLibraryUtility.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabSongLibraryUtility.ImageIndex = 10;
            this.tabSongLibraryUtility.Location = new System.Drawing.Point(4, 24);
            this.tabSongLibraryUtility.Name = "tabSongLibraryUtility";
            this.tabSongLibraryUtility.Padding = new System.Windows.Forms.Padding(3);
            this.tabSongLibraryUtility.Size = new System.Drawing.Size(1095, 522);
            this.tabSongLibraryUtility.TabIndex = 8;
            this.tabSongLibraryUtility.Text = "Song Library / Utility";
            // 
            // groupBox39
            // 
            this.groupBox39.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox39.Controls.Add(this.groupBox48);
            this.groupBox39.Controls.Add(this.buttonSongLibListFilterReset);
            this.groupBox39.Controls.Add(this.label55);
            this.groupBox39.Controls.Add(this.textBoxSongLibListFilter);
            this.groupBox39.Controls.Add(this.listBoxSongLibrary);
            this.groupBox39.Controls.Add(this.buttonCreateSongFromOpenMidi);
            this.groupBox39.Controls.Add(this.button68);
            this.groupBox39.Controls.Add(this.button69);
            this.groupBox39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox39.Location = new System.Drawing.Point(9, 6);
            this.groupBox39.Name = "groupBox39";
            this.groupBox39.Size = new System.Drawing.Size(262, 510);
            this.groupBox39.TabIndex = 17;
            this.groupBox39.TabStop = false;
            this.groupBox39.Text = "Song Library";
            // 
            // groupBox48
            // 
            this.groupBox48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox48.Controls.Add(this.checkBoxSongLibSongListSortAscending);
            this.groupBox48.Controls.Add(this.radioSongLibSongListSortCompleted);
            this.groupBox48.Controls.Add(this.radioSongLibSongListSortID);
            this.groupBox48.Controls.Add(this.radioSongLibSongListSortName);
            this.groupBox48.Location = new System.Drawing.Point(6, 383);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Size = new System.Drawing.Size(250, 76);
            this.groupBox48.TabIndex = 7;
            this.groupBox48.TabStop = false;
            this.groupBox48.Text = "Sort";
            // 
            // checkBoxSongLibSongListSortAscending
            // 
            this.checkBoxSongLibSongListSortAscending.Checked = true;
            this.checkBoxSongLibSongListSortAscending.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSongLibSongListSortAscending.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongLibSongListSortAscending.Location = new System.Drawing.Point(6, 47);
            this.checkBoxSongLibSongListSortAscending.Name = "checkBoxSongLibSongListSortAscending";
            this.checkBoxSongLibSongListSortAscending.Size = new System.Drawing.Size(82, 19);
            this.checkBoxSongLibSongListSortAscending.TabIndex = 3;
            this.checkBoxSongLibSongListSortAscending.Text = "Ascending";
            this.checkBoxSongLibSongListSortAscending.UseVisualStyleBackColor = true;
            this.checkBoxSongLibSongListSortAscending.CheckedChanged += new System.EventHandler(this.checkBoxSongLibSongListSortAscending_CheckedChanged);
            // 
            // radioSongLibSongListSortCompleted
            // 
            this.radioSongLibSongListSortCompleted.ForeColor = System.Drawing.Color.Black;
            this.radioSongLibSongListSortCompleted.Location = new System.Drawing.Point(114, 22);
            this.radioSongLibSongListSortCompleted.Name = "radioSongLibSongListSortCompleted";
            this.radioSongLibSongListSortCompleted.Size = new System.Drawing.Size(84, 19);
            this.radioSongLibSongListSortCompleted.TabIndex = 2;
            this.radioSongLibSongListSortCompleted.Text = "Completed";
            this.radioSongLibSongListSortCompleted.UseVisualStyleBackColor = true;
            this.radioSongLibSongListSortCompleted.CheckedChanged += new System.EventHandler(this.radioSongLibSongListSortCompleted_CheckedChanged);
            // 
            // radioSongLibSongListSortID
            // 
            this.radioSongLibSongListSortID.ForeColor = System.Drawing.Color.Black;
            this.radioSongLibSongListSortID.Location = new System.Drawing.Point(72, 22);
            this.radioSongLibSongListSortID.Name = "radioSongLibSongListSortID";
            this.radioSongLibSongListSortID.Size = new System.Drawing.Size(36, 19);
            this.radioSongLibSongListSortID.TabIndex = 1;
            this.radioSongLibSongListSortID.Text = "ID";
            this.radioSongLibSongListSortID.UseVisualStyleBackColor = true;
            this.radioSongLibSongListSortID.CheckedChanged += new System.EventHandler(this.radioSongLibSongListSortID_CheckedChanged);
            // 
            // radioSongLibSongListSortName
            // 
            this.radioSongLibSongListSortName.Checked = true;
            this.radioSongLibSongListSortName.ForeColor = System.Drawing.Color.Black;
            this.radioSongLibSongListSortName.Location = new System.Drawing.Point(6, 22);
            this.radioSongLibSongListSortName.Name = "radioSongLibSongListSortName";
            this.radioSongLibSongListSortName.Size = new System.Drawing.Size(57, 19);
            this.radioSongLibSongListSortName.TabIndex = 0;
            this.radioSongLibSongListSortName.TabStop = true;
            this.radioSongLibSongListSortName.Text = "Name";
            this.radioSongLibSongListSortName.UseVisualStyleBackColor = true;
            this.radioSongLibSongListSortName.CheckedChanged += new System.EventHandler(this.radioSongLibSongListSortName_CheckedChanged);
            // 
            // buttonSongLibListFilterReset
            // 
            this.buttonSongLibListFilterReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSongLibListFilterReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSongLibListFilterReset.Image = global::ProUpgradeEditor.ProResources.ui_text_field_clear;
            this.buttonSongLibListFilterReset.Location = new System.Drawing.Point(232, 354);
            this.buttonSongLibListFilterReset.Name = "buttonSongLibListFilterReset";
            this.buttonSongLibListFilterReset.Size = new System.Drawing.Size(24, 24);
            this.buttonSongLibListFilterReset.TabIndex = 6;
            this.toolTip1.SetToolTip(this.buttonSongLibListFilterReset, "Clear Filter");
            this.buttonSongLibListFilterReset.UseVisualStyleBackColor = true;
            this.buttonSongLibListFilterReset.Click += new System.EventHandler(this.buttonSongLibListFilterReset_Click);
            // 
            // label55
            // 
            this.label55.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label55.AutoSize = true;
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.Location = new System.Drawing.Point(42, 357);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(36, 15);
            this.label55.TabIndex = 5;
            this.label55.Text = "Filter:";
            // 
            // textBoxSongLibListFilter
            // 
            this.textBoxSongLibListFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSongLibListFilter.Location = new System.Drawing.Point(84, 354);
            this.textBoxSongLibListFilter.Name = "textBoxSongLibListFilter";
            this.textBoxSongLibListFilter.Size = new System.Drawing.Size(142, 23);
            this.textBoxSongLibListFilter.TabIndex = 4;
            this.textBoxSongLibListFilter.TextChanged += new System.EventHandler(this.textBoxSongLibListFilter_TextChanged);
            // 
            // listBoxSongLibrary
            // 
            this.listBoxSongLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxSongLibrary.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxSongLibrary.FormattingEnabled = true;
            this.listBoxSongLibrary.ItemHeight = 15;
            this.listBoxSongLibrary.Location = new System.Drawing.Point(6, 54);
            this.listBoxSongLibrary.Name = "listBoxSongLibrary";
            this.listBoxSongLibrary.ScrollAlwaysVisible = true;
            this.listBoxSongLibrary.Size = new System.Drawing.Size(250, 259);
            this.listBoxSongLibrary.TabIndex = 1;
            this.listBoxSongLibrary.DoubleClick += new System.EventHandler(this.listBoxSongLibrary_DoubleClick);
            // 
            // buttonCreateSongFromOpenMidi
            // 
            this.buttonCreateSongFromOpenMidi.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCreateSongFromOpenMidi.ImageIndex = 14;
            this.buttonCreateSongFromOpenMidi.ImageList = this.imageList1;
            this.buttonCreateSongFromOpenMidi.Location = new System.Drawing.Point(6, 24);
            this.buttonCreateSongFromOpenMidi.Name = "buttonCreateSongFromOpenMidi";
            this.buttonCreateSongFromOpenMidi.Size = new System.Drawing.Size(167, 24);
            this.buttonCreateSongFromOpenMidi.TabIndex = 0;
            this.buttonCreateSongFromOpenMidi.Text = "Create From Open Midi";
            this.buttonCreateSongFromOpenMidi.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCreateSongFromOpenMidi.UseVisualStyleBackColor = true;
            this.buttonCreateSongFromOpenMidi.Click += new System.EventHandler(this.buttonCreateSongFromOpenMidi_Click);
            // 
            // button68
            // 
            this.button68.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button68.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button68.ImageIndex = 49;
            this.button68.ImageList = this.imageList1;
            this.button68.Location = new System.Drawing.Point(232, 319);
            this.button68.Name = "button68";
            this.button68.Size = new System.Drawing.Size(24, 24);
            this.button68.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button68, "Open Selected Song");
            this.button68.UseVisualStyleBackColor = true;
            this.button68.Click += new System.EventHandler(this.button68_Click);
            // 
            // button69
            // 
            this.button69.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button69.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button69.ImageIndex = 31;
            this.button69.ImageList = this.imageList1;
            this.button69.Location = new System.Drawing.Point(6, 319);
            this.button69.Name = "button69";
            this.button69.Size = new System.Drawing.Size(24, 24);
            this.button69.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button69, "Remove Selected Song");
            this.button69.UseVisualStyleBackColor = true;
            this.button69.Click += new System.EventHandler(this.button69_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabSongLibSongProperties);
            this.tabControl2.Controls.Add(this.tabSongLibUtility);
            this.tabControl2.Controls.Add(this.tabSongLibSongUtility);
            this.tabControl2.ImageList = this.imageList1;
            this.tabControl2.Location = new System.Drawing.Point(277, 6);
            this.tabControl2.MinimumSize = new System.Drawing.Size(736, 403);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(818, 516);
            this.tabControl2.TabIndex = 16;
            // 
            // tabSongLibSongProperties
            // 
            this.tabSongLibSongProperties.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabSongLibSongProperties.Controls.Add(this.groupBox25);
            this.tabSongLibSongProperties.ImageIndex = 10;
            this.tabSongLibSongProperties.Location = new System.Drawing.Point(4, 24);
            this.tabSongLibSongProperties.Margin = new System.Windows.Forms.Padding(0);
            this.tabSongLibSongProperties.Name = "tabSongLibSongProperties";
            this.tabSongLibSongProperties.Size = new System.Drawing.Size(810, 488);
            this.tabSongLibSongProperties.TabIndex = 0;
            this.tabSongLibSongProperties.Text = "Song Properties";
            // 
            // groupBox25
            // 
            this.groupBox25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.groupBox25.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox25.Controls.Add(this.buttonSongPropertiesViewMp3Preview);
            this.groupBox25.Controls.Add(this.buttonFindMP3Offset);
            this.groupBox25.Controls.Add(this.groupBox46);
            this.groupBox25.Controls.Add(this.groupBox45);
            this.groupBox25.Controls.Add(this.checkBoxSongPropertiesEnableMP3Playback);
            this.groupBox25.Controls.Add(this.trackBarMP3Volume);
            this.groupBox25.Controls.Add(this.checkBoxEnableMidiPlayback);
            this.groupBox25.Controls.Add(this.trackBarMidiVolume);
            this.groupBox25.Controls.Add(this.textBoxSongPropertiesMP3StartOffset);
            this.groupBox25.Controls.Add(this.label54);
            this.groupBox25.Controls.Add(this.buttonSongPropertiesExploreMP3Location);
            this.groupBox25.Controls.Add(this.buttonSongPropertiesChooseMP3Location);
            this.groupBox25.Controls.Add(this.label53);
            this.groupBox25.Controls.Add(this.textBoxSongPropertiesMP3Location);
            this.groupBox25.Controls.Add(this.buttonSongLibCopyPackageToUSB);
            this.groupBox25.Controls.Add(this.groupBox35);
            this.groupBox25.Controls.Add(this.buttonSongLibViewPackageContents);
            this.groupBox25.Controls.Add(this.buttonSongPropertiesCheckPackage);
            this.groupBox25.Controls.Add(this.buttonRebuildPackage);
            this.groupBox25.Controls.Add(this.groupBox28);
            this.groupBox25.Controls.Add(this.label36);
            this.groupBox25.Controls.Add(this.groupBox1);
            this.groupBox25.Controls.Add(this.button80);
            this.groupBox25.Controls.Add(this.button66);
            this.groupBox25.Controls.Add(this.button79);
            this.groupBox25.Controls.Add(this.button71);
            this.groupBox25.Controls.Add(this.button78);
            this.groupBox25.Controls.Add(this.button65);
            this.groupBox25.Controls.Add(this.buttonSongPropertiesSaveChanges);
            this.groupBox25.Controls.Add(this.label37);
            this.groupBox25.Controls.Add(this.label34);
            this.groupBox25.Controls.Add(this.textBoxSongLibConFile);
            this.groupBox25.Controls.Add(this.textBoxSongLibProMidiFileName);
            this.groupBox25.Controls.Add(this.label35);
            this.groupBox25.Controls.Add(this.label38);
            this.groupBox25.Controls.Add(this.label39);
            this.groupBox25.Controls.Add(this.textBox24);
            this.groupBox25.Controls.Add(this.textBoxSongLibG5MidiFileName);
            this.groupBox25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox25.Location = new System.Drawing.Point(0, 0);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(810, 488);
            this.groupBox25.TabIndex = 0;
            this.groupBox25.TabStop = false;
            this.groupBox25.UseCompatibleTextRendering = true;
            this.groupBox25.Enter += new System.EventHandler(this.groupBox25_Enter);
            // 
            // buttonSongPropertiesViewMp3Preview
            // 
            this.buttonSongPropertiesViewMp3Preview.ImageIndex = 49;
            this.buttonSongPropertiesViewMp3Preview.ImageList = this.imageList1;
            this.buttonSongPropertiesViewMp3Preview.Location = new System.Drawing.Point(362, 137);
            this.buttonSongPropertiesViewMp3Preview.Name = "buttonSongPropertiesViewMp3Preview";
            this.buttonSongPropertiesViewMp3Preview.Size = new System.Drawing.Size(25, 24);
            this.buttonSongPropertiesViewMp3Preview.TabIndex = 35;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesViewMp3Preview, "View MP3 Preview");
            this.buttonSongPropertiesViewMp3Preview.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesViewMp3Preview.Click += new System.EventHandler(this.buttonSongPropertiesViewMp3Preview_Click);
            // 
            // buttonFindMP3Offset
            // 
            this.buttonFindMP3Offset.ImageIndex = 29;
            this.buttonFindMP3Offset.ImageList = this.imageList1;
            this.buttonFindMP3Offset.Location = new System.Drawing.Point(313, 163);
            this.buttonFindMP3Offset.Name = "buttonFindMP3Offset";
            this.buttonFindMP3Offset.Size = new System.Drawing.Size(24, 24);
            this.buttonFindMP3Offset.TabIndex = 34;
            this.toolTip1.SetToolTip(this.buttonFindMP3Offset, "Attempt finding offset");
            this.buttonFindMP3Offset.UseVisualStyleBackColor = true;
            this.buttonFindMP3Offset.Visible = false;
            this.buttonFindMP3Offset.Click += new System.EventHandler(this.buttonFindMP3Offset_Click);
            // 
            // groupBox46
            // 
            this.groupBox46.Controls.Add(this.checkBoxAutoGenGuitarEasy);
            this.groupBox46.Controls.Add(this.checkBoxAutoGenGuitarMedium);
            this.groupBox46.Controls.Add(this.checkBoxAutoGenGuitarHard);
            this.groupBox46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox46.Location = new System.Drawing.Point(6, 255);
            this.groupBox46.Name = "groupBox46";
            this.groupBox46.Size = new System.Drawing.Size(200, 44);
            this.groupBox46.TabIndex = 8;
            this.groupBox46.TabStop = false;
            this.groupBox46.Text = "Guitar Auto Difficulty Generation";
            // 
            // checkBoxAutoGenGuitarEasy
            // 
            this.checkBoxAutoGenGuitarEasy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenGuitarEasy.Location = new System.Drawing.Point(137, 19);
            this.checkBoxAutoGenGuitarEasy.Name = "checkBoxAutoGenGuitarEasy";
            this.checkBoxAutoGenGuitarEasy.Size = new System.Drawing.Size(49, 19);
            this.checkBoxAutoGenGuitarEasy.TabIndex = 8;
            this.checkBoxAutoGenGuitarEasy.Text = "Easy";
            this.checkBoxAutoGenGuitarEasy.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenGuitarEasy.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenGuitarEasy_CheckedChanged);
            // 
            // checkBoxAutoGenGuitarMedium
            // 
            this.checkBoxAutoGenGuitarMedium.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenGuitarMedium.Location = new System.Drawing.Point(62, 19);
            this.checkBoxAutoGenGuitarMedium.Name = "checkBoxAutoGenGuitarMedium";
            this.checkBoxAutoGenGuitarMedium.Size = new System.Drawing.Size(71, 19);
            this.checkBoxAutoGenGuitarMedium.TabIndex = 7;
            this.checkBoxAutoGenGuitarMedium.Text = "Medium";
            this.checkBoxAutoGenGuitarMedium.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenGuitarMedium.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenGuitarMedium_CheckedChanged);
            // 
            // checkBoxAutoGenGuitarHard
            // 
            this.checkBoxAutoGenGuitarHard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenGuitarHard.Location = new System.Drawing.Point(6, 19);
            this.checkBoxAutoGenGuitarHard.Name = "checkBoxAutoGenGuitarHard";
            this.checkBoxAutoGenGuitarHard.Size = new System.Drawing.Size(52, 19);
            this.checkBoxAutoGenGuitarHard.TabIndex = 6;
            this.checkBoxAutoGenGuitarHard.Text = "Hard";
            this.checkBoxAutoGenGuitarHard.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenGuitarHard.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenGuitarHard_CheckedChanged);
            // 
            // groupBox45
            // 
            this.groupBox45.Controls.Add(this.checkBoxAutoGenBassEasy);
            this.groupBox45.Controls.Add(this.checkBoxAutoGenBassMedium);
            this.groupBox45.Controls.Add(this.checkBoxAutoGenBassHard);
            this.groupBox45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox45.Location = new System.Drawing.Point(212, 255);
            this.groupBox45.Name = "groupBox45";
            this.groupBox45.Size = new System.Drawing.Size(200, 44);
            this.groupBox45.TabIndex = 7;
            this.groupBox45.TabStop = false;
            this.groupBox45.Text = "Bass Auto Difficulty Generation";
            // 
            // checkBoxAutoGenBassEasy
            // 
            this.checkBoxAutoGenBassEasy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenBassEasy.Location = new System.Drawing.Point(135, 19);
            this.checkBoxAutoGenBassEasy.Name = "checkBoxAutoGenBassEasy";
            this.checkBoxAutoGenBassEasy.Size = new System.Drawing.Size(49, 19);
            this.checkBoxAutoGenBassEasy.TabIndex = 10;
            this.checkBoxAutoGenBassEasy.Text = "Easy";
            this.checkBoxAutoGenBassEasy.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenBassEasy.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenBassEasy_CheckedChanged);
            // 
            // checkBoxAutoGenBassMedium
            // 
            this.checkBoxAutoGenBassMedium.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenBassMedium.Location = new System.Drawing.Point(62, 19);
            this.checkBoxAutoGenBassMedium.Name = "checkBoxAutoGenBassMedium";
            this.checkBoxAutoGenBassMedium.Size = new System.Drawing.Size(71, 19);
            this.checkBoxAutoGenBassMedium.TabIndex = 9;
            this.checkBoxAutoGenBassMedium.Text = "Medium";
            this.checkBoxAutoGenBassMedium.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenBassMedium.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenBassMedium_CheckedChanged);
            // 
            // checkBoxAutoGenBassHard
            // 
            this.checkBoxAutoGenBassHard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoGenBassHard.Location = new System.Drawing.Point(6, 19);
            this.checkBoxAutoGenBassHard.Name = "checkBoxAutoGenBassHard";
            this.checkBoxAutoGenBassHard.Size = new System.Drawing.Size(52, 19);
            this.checkBoxAutoGenBassHard.TabIndex = 6;
            this.checkBoxAutoGenBassHard.Text = "Hard";
            this.checkBoxAutoGenBassHard.UseVisualStyleBackColor = true;
            this.checkBoxAutoGenBassHard.CheckedChanged += new System.EventHandler(this.checkBoxAutoGenBassHard_CheckedChanged);
            // 
            // checkBoxSongPropertiesEnableMP3Playback
            // 
            this.checkBoxSongPropertiesEnableMP3Playback.AutoSize = true;
            this.checkBoxSongPropertiesEnableMP3Playback.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongPropertiesEnableMP3Playback.Location = new System.Drawing.Point(267, 312);
            this.checkBoxSongPropertiesEnableMP3Playback.Name = "checkBoxSongPropertiesEnableMP3Playback";
            this.checkBoxSongPropertiesEnableMP3Playback.Size = new System.Drawing.Size(138, 19);
            this.checkBoxSongPropertiesEnableMP3Playback.TabIndex = 32;
            this.checkBoxSongPropertiesEnableMP3Playback.Text = "Enable MP3 Playback";
            this.checkBoxSongPropertiesEnableMP3Playback.UseVisualStyleBackColor = true;
            this.checkBoxSongPropertiesEnableMP3Playback.CheckedChanged += new System.EventHandler(this.checkBoxSongPropertiesEnableMP3Playback_CheckedChanged);
            // 
            // trackBarMP3Volume
            // 
            this.trackBarMP3Volume.Location = new System.Drawing.Point(267, 334);
            this.trackBarMP3Volume.Maximum = 100;
            this.trackBarMP3Volume.Name = "trackBarMP3Volume";
            this.trackBarMP3Volume.Size = new System.Drawing.Size(156, 45);
            this.trackBarMP3Volume.SmallChange = 10;
            this.trackBarMP3Volume.TabIndex = 33;
            this.trackBarMP3Volume.TickFrequency = 10;
            this.trackBarMP3Volume.Value = 100;
            this.trackBarMP3Volume.Scroll += new System.EventHandler(this.trackBarMP3Volume_Scroll);
            // 
            // checkBoxEnableMidiPlayback
            // 
            this.checkBoxEnableMidiPlayback.AutoSize = true;
            this.checkBoxEnableMidiPlayback.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxEnableMidiPlayback.Location = new System.Drawing.Point(108, 312);
            this.checkBoxEnableMidiPlayback.Name = "checkBoxEnableMidiPlayback";
            this.checkBoxEnableMidiPlayback.Size = new System.Drawing.Size(138, 19);
            this.checkBoxEnableMidiPlayback.TabIndex = 32;
            this.checkBoxEnableMidiPlayback.Text = "Enable Midi Playback";
            this.checkBoxEnableMidiPlayback.UseVisualStyleBackColor = true;
            this.checkBoxEnableMidiPlayback.CheckedChanged += new System.EventHandler(this.checkBoxEnableMidiPlayback_CheckedChanged);
            // 
            // trackBarMidiVolume
            // 
            this.trackBarMidiVolume.Location = new System.Drawing.Point(108, 334);
            this.trackBarMidiVolume.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarMidiVolume.Maximum = 100;
            this.trackBarMidiVolume.Name = "trackBarMidiVolume";
            this.trackBarMidiVolume.Size = new System.Drawing.Size(156, 45);
            this.trackBarMidiVolume.SmallChange = 10;
            this.trackBarMidiVolume.TabIndex = 32;
            this.trackBarMidiVolume.TickFrequency = 10;
            this.trackBarMidiVolume.Value = 100;
            this.trackBarMidiVolume.Scroll += new System.EventHandler(this.trackBarMidiVolume_Scroll);
            // 
            // textBoxSongPropertiesMP3StartOffset
            // 
            this.textBoxSongPropertiesMP3StartOffset.Location = new System.Drawing.Point(211, 166);
            this.textBoxSongPropertiesMP3StartOffset.Name = "textBoxSongPropertiesMP3StartOffset";
            this.textBoxSongPropertiesMP3StartOffset.Size = new System.Drawing.Size(100, 23);
            this.textBoxSongPropertiesMP3StartOffset.TabIndex = 31;
            this.textBoxSongPropertiesMP3StartOffset.Text = "0";
            this.textBoxSongPropertiesMP3StartOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.textBoxSongPropertiesMP3StartOffset, "Time in Milliseconds to adjust playback. Negative to start playback late. Positiv" +
        "e to start playback early.");
            this.textBoxSongPropertiesMP3StartOffset.TextChanged += new System.EventHandler(this.textBoxSongPropertiesMP3StartOffset_TextChanged);
            this.textBoxSongPropertiesMP3StartOffset.Leave += new System.EventHandler(this.textBoxSongPropertiesMP3StartOffset_Leave);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(112, 170);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(96, 15);
            this.label54.TabIndex = 30;
            this.label54.Text = "MP3 Start Offset:";
            // 
            // buttonSongPropertiesExploreMP3Location
            // 
            this.buttonSongPropertiesExploreMP3Location.ImageKey = "folder_explore.png";
            this.buttonSongPropertiesExploreMP3Location.ImageList = this.imageList1;
            this.buttonSongPropertiesExploreMP3Location.Location = new System.Drawing.Point(338, 137);
            this.buttonSongPropertiesExploreMP3Location.Name = "buttonSongPropertiesExploreMP3Location";
            this.buttonSongPropertiesExploreMP3Location.Size = new System.Drawing.Size(24, 24);
            this.buttonSongPropertiesExploreMP3Location.TabIndex = 17;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesExploreMP3Location, "Explore File Location");
            this.buttonSongPropertiesExploreMP3Location.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesExploreMP3Location.Click += new System.EventHandler(this.buttonSongPropertiesExploreMP3Location_Click);
            // 
            // buttonSongPropertiesChooseMP3Location
            // 
            this.buttonSongPropertiesChooseMP3Location.ImageIndex = 29;
            this.buttonSongPropertiesChooseMP3Location.ImageList = this.imageList1;
            this.buttonSongPropertiesChooseMP3Location.Location = new System.Drawing.Point(313, 137);
            this.buttonSongPropertiesChooseMP3Location.Name = "buttonSongPropertiesChooseMP3Location";
            this.buttonSongPropertiesChooseMP3Location.Size = new System.Drawing.Size(24, 24);
            this.buttonSongPropertiesChooseMP3Location.TabIndex = 16;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesChooseMP3Location, "Select File");
            this.buttonSongPropertiesChooseMP3Location.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesChooseMP3Location.Click += new System.EventHandler(this.buttonSongPropertiesChooseMP3Location_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(24, 144);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(83, 15);
            this.label53.TabIndex = 29;
            this.label53.Text = "MP3 Location:";
            // 
            // textBoxSongPropertiesMP3Location
            // 
            this.textBoxSongPropertiesMP3Location.Location = new System.Drawing.Point(113, 140);
            this.textBoxSongPropertiesMP3Location.Name = "textBoxSongPropertiesMP3Location";
            this.textBoxSongPropertiesMP3Location.Size = new System.Drawing.Size(198, 23);
            this.textBoxSongPropertiesMP3Location.TabIndex = 15;
            this.textBoxSongPropertiesMP3Location.TextChanged += new System.EventHandler(this.textBoxSongPropertiesMP3Location_TextChanged);
            // 
            // buttonSongLibCopyPackageToUSB
            // 
            this.buttonSongLibCopyPackageToUSB.ImageIndex = 21;
            this.buttonSongLibCopyPackageToUSB.ImageList = this.imageList1;
            this.buttonSongLibCopyPackageToUSB.Location = new System.Drawing.Point(437, 112);
            this.buttonSongLibCopyPackageToUSB.Name = "buttonSongLibCopyPackageToUSB";
            this.buttonSongLibCopyPackageToUSB.Size = new System.Drawing.Size(25, 24);
            this.buttonSongLibCopyPackageToUSB.TabIndex = 14;
            this.toolTip1.SetToolTip(this.buttonSongLibCopyPackageToUSB, "Copy Package to USB");
            this.buttonSongLibCopyPackageToUSB.UseVisualStyleBackColor = true;
            this.buttonSongLibCopyPackageToUSB.Click += new System.EventHandler(this.button129_Click);
            // 
            // groupBox35
            // 
            this.groupBox35.Controls.Add(this.checkBoxSongLibHasBass);
            this.groupBox35.Controls.Add(this.checkBoxSongLibHasGuitar);
            this.groupBox35.Controls.Add(this.checkBoxSongLibCopyGuitar);
            this.groupBox35.Controls.Add(this.checkBoxSongLibIsComplete);
            this.groupBox35.Controls.Add(this.checkBoxSongLibIsFinalized);
            this.groupBox35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox35.Location = new System.Drawing.Point(6, 197);
            this.groupBox35.Name = "groupBox35";
            this.groupBox35.Size = new System.Drawing.Size(517, 49);
            this.groupBox35.TabIndex = 27;
            this.groupBox35.TabStop = false;
            this.groupBox35.Text = "Editor Settings";
            // 
            // checkBoxSongLibHasBass
            // 
            this.checkBoxSongLibHasBass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongLibHasBass.Location = new System.Drawing.Point(6, 19);
            this.checkBoxSongLibHasBass.Name = "checkBoxSongLibHasBass";
            this.checkBoxSongLibHasBass.Size = new System.Drawing.Size(73, 19);
            this.checkBoxSongLibHasBass.TabIndex = 0;
            this.checkBoxSongLibHasBass.Text = "Has Bass";
            this.checkBoxSongLibHasBass.UseVisualStyleBackColor = true;
            this.checkBoxSongLibHasBass.CheckedChanged += new System.EventHandler(this.checkBoxSongLibHasBass_CheckedChanged);
            // 
            // checkBoxSongLibHasGuitar
            // 
            this.checkBoxSongLibHasGuitar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongLibHasGuitar.Location = new System.Drawing.Point(83, 19);
            this.checkBoxSongLibHasGuitar.Name = "checkBoxSongLibHasGuitar";
            this.checkBoxSongLibHasGuitar.Size = new System.Drawing.Size(82, 19);
            this.checkBoxSongLibHasGuitar.TabIndex = 1;
            this.checkBoxSongLibHasGuitar.Text = "Has Guitar";
            this.checkBoxSongLibHasGuitar.UseVisualStyleBackColor = true;
            this.checkBoxSongLibHasGuitar.CheckedChanged += new System.EventHandler(this.checkBoxSongLibHasGuitar_CheckedChanged);
            // 
            // checkBoxSongLibCopyGuitar
            // 
            this.checkBoxSongLibCopyGuitar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongLibCopyGuitar.Location = new System.Drawing.Point(169, 19);
            this.checkBoxSongLibCopyGuitar.Name = "checkBoxSongLibCopyGuitar";
            this.checkBoxSongLibCopyGuitar.Size = new System.Drawing.Size(133, 19);
            this.checkBoxSongLibCopyGuitar.TabIndex = 3;
            this.checkBoxSongLibCopyGuitar.Text = "Copy Guitar To Bass";
            this.checkBoxSongLibCopyGuitar.UseVisualStyleBackColor = true;
            this.checkBoxSongLibCopyGuitar.CheckedChanged += new System.EventHandler(this.checkBoxSongLibCopyGuitar_CheckedChanged);
            // 
            // checkBoxSongLibIsComplete
            // 
            this.checkBoxSongLibIsComplete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongLibIsComplete.Location = new System.Drawing.Point(307, 19);
            this.checkBoxSongLibIsComplete.Name = "checkBoxSongLibIsComplete";
            this.checkBoxSongLibIsComplete.Size = new System.Drawing.Size(86, 19);
            this.checkBoxSongLibIsComplete.TabIndex = 4;
            this.checkBoxSongLibIsComplete.Text = "Completed";
            this.toolTip1.SetToolTip(this.checkBoxSongLibIsComplete, "Use this song in batch operations in the Utility tab");
            this.checkBoxSongLibIsComplete.UseVisualStyleBackColor = true;
            this.checkBoxSongLibIsComplete.CheckedChanged += new System.EventHandler(this.checkBoxSongLibIsComplete_CheckedChanged);
            // 
            // checkBoxSongLibIsFinalized
            // 
            this.checkBoxSongLibIsFinalized.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongLibIsFinalized.Location = new System.Drawing.Point(399, 19);
            this.checkBoxSongLibIsFinalized.Name = "checkBoxSongLibIsFinalized";
            this.checkBoxSongLibIsFinalized.Size = new System.Drawing.Size(77, 19);
            this.checkBoxSongLibIsFinalized.TabIndex = 2;
            this.checkBoxSongLibIsFinalized.Text = "Finalized";
            this.toolTip1.SetToolTip(this.checkBoxSongLibIsFinalized, "Do Not modify this song any more.");
            this.checkBoxSongLibIsFinalized.UseVisualStyleBackColor = true;
            this.checkBoxSongLibIsFinalized.CheckedChanged += new System.EventHandler(this.checkBoxSongLibIsFinalized_CheckedChanged);
            // 
            // buttonSongLibViewPackageContents
            // 
            this.buttonSongLibViewPackageContents.ImageIndex = 49;
            this.buttonSongLibViewPackageContents.ImageList = this.imageList1;
            this.buttonSongLibViewPackageContents.Location = new System.Drawing.Point(412, 112);
            this.buttonSongLibViewPackageContents.Name = "buttonSongLibViewPackageContents";
            this.buttonSongLibViewPackageContents.Size = new System.Drawing.Size(25, 24);
            this.buttonSongLibViewPackageContents.TabIndex = 13;
            this.toolTip1.SetToolTip(this.buttonSongLibViewPackageContents, "View Package Contents");
            this.buttonSongLibViewPackageContents.UseVisualStyleBackColor = true;
            this.buttonSongLibViewPackageContents.Click += new System.EventHandler(this.button107_Click);
            // 
            // buttonSongPropertiesCheckPackage
            // 
            this.buttonSongPropertiesCheckPackage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSongPropertiesCheckPackage.ImageIndex = 16;
            this.buttonSongPropertiesCheckPackage.ImageList = this.imageList1;
            this.buttonSongPropertiesCheckPackage.Location = new System.Drawing.Point(387, 112);
            this.buttonSongPropertiesCheckPackage.Name = "buttonSongPropertiesCheckPackage";
            this.buttonSongPropertiesCheckPackage.Size = new System.Drawing.Size(25, 24);
            this.buttonSongPropertiesCheckPackage.TabIndex = 12;
            this.buttonSongPropertiesCheckPackage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesCheckPackage, "Check Package for Errors");
            this.buttonSongPropertiesCheckPackage.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesCheckPackage.Click += new System.EventHandler(this.button100_Click);
            // 
            // buttonRebuildPackage
            // 
            this.buttonRebuildPackage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRebuildPackage.ImageIndex = 52;
            this.buttonRebuildPackage.ImageList = this.imageList1;
            this.buttonRebuildPackage.Location = new System.Drawing.Point(362, 112);
            this.buttonRebuildPackage.Name = "buttonRebuildPackage";
            this.buttonRebuildPackage.Size = new System.Drawing.Size(25, 24);
            this.buttonRebuildPackage.TabIndex = 11;
            this.toolTip1.SetToolTip(this.buttonRebuildPackage, "Rebuild Package");
            this.buttonRebuildPackage.UseVisualStyleBackColor = true;
            this.buttonRebuildPackage.Click += new System.EventHandler(this.buttonRebuildPackage_Click);
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.buttonSongPropertiesMidiPause);
            this.groupBox28.Controls.Add(this.buttonSongPropertiesMidiPlay);
            this.groupBox28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox28.Location = new System.Drawing.Point(6, 305);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(92, 55);
            this.groupBox28.TabIndex = 18;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Playback";
            // 
            // buttonSongPropertiesMidiPause
            // 
            this.buttonSongPropertiesMidiPause.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongPropertiesMidiPause.ImageIndex = 76;
            this.buttonSongPropertiesMidiPause.ImageList = this.imageList1;
            this.buttonSongPropertiesMidiPause.Location = new System.Drawing.Point(45, 19);
            this.buttonSongPropertiesMidiPause.Name = "buttonSongPropertiesMidiPause";
            this.buttonSongPropertiesMidiPause.Size = new System.Drawing.Size(24, 24);
            this.buttonSongPropertiesMidiPause.TabIndex = 1;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesMidiPause, "Stop");
            this.buttonSongPropertiesMidiPause.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesMidiPause.Click += new System.EventHandler(this.buttonSongPropertiesMidiPause_Click);
            // 
            // buttonSongPropertiesMidiPlay
            // 
            this.buttonSongPropertiesMidiPlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongPropertiesMidiPlay.ImageIndex = 77;
            this.buttonSongPropertiesMidiPlay.ImageList = this.imageList1;
            this.buttonSongPropertiesMidiPlay.Location = new System.Drawing.Point(16, 19);
            this.buttonSongPropertiesMidiPlay.Name = "buttonSongPropertiesMidiPlay";
            this.buttonSongPropertiesMidiPlay.Size = new System.Drawing.Size(24, 24);
            this.buttonSongPropertiesMidiPlay.TabIndex = 0;
            this.toolTip1.SetToolTip(this.buttonSongPropertiesMidiPlay, "Play");
            this.buttonSongPropertiesMidiPlay.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesMidiPlay.Click += new System.EventHandler(this.buttonSongPropertiesMidiPlay_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(72, 19);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(37, 15);
            this.label36.TabIndex = 0;
            this.label36.Text = "Song:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button61);
            this.groupBox1.Controls.Add(this.groupBox27);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.groupBox26);
            this.groupBox1.Controls.Add(this.textBoxCONShortName);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.textBoxCONSongID);
            this.groupBox1.Controls.Add(this.comboProGDifficulty);
            this.groupBox1.Controls.Add(this.comboProBDifficulty);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox1.Location = new System.Drawing.Point(529, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 254);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DTA File Properties";
            // 
            // button61
            // 
            this.button61.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button61.ImageIndex = 29;
            this.button61.ImageList = this.imageList1;
            this.button61.Location = new System.Drawing.Point(237, 48);
            this.button61.Margin = new System.Windows.Forms.Padding(0);
            this.button61.Name = "button61";
            this.button61.Size = new System.Drawing.Size(24, 24);
            this.button61.TabIndex = 4;
            this.toolTip1.SetToolTip(this.button61, "Searches in the folder of the .mid and up two folders for the DTA information");
            this.button61.UseVisualStyleBackColor = true;
            this.button61.Click += new System.EventHandler(this.button61_Click);
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.textBox43);
            this.groupBox27.Controls.Add(this.textBox44);
            this.groupBox27.Controls.Add(this.textBox45);
            this.groupBox27.Controls.Add(this.textBox46);
            this.groupBox27.Controls.Add(this.textBox47);
            this.groupBox27.Controls.Add(this.textBox48);
            this.groupBox27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox27.Location = new System.Drawing.Point(14, 182);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(225, 49);
            this.groupBox27.TabIndex = 10;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Bass String Tuning";
            // 
            // textBox43
            // 
            this.textBox43.Location = new System.Drawing.Point(8, 19);
            this.textBox43.Name = "textBox43";
            this.textBox43.Size = new System.Drawing.Size(30, 23);
            this.textBox43.TabIndex = 1;
            // 
            // textBox44
            // 
            this.textBox44.Location = new System.Drawing.Point(44, 19);
            this.textBox44.Name = "textBox44";
            this.textBox44.Size = new System.Drawing.Size(30, 23);
            this.textBox44.TabIndex = 2;
            // 
            // textBox45
            // 
            this.textBox45.Location = new System.Drawing.Point(80, 19);
            this.textBox45.Name = "textBox45";
            this.textBox45.Size = new System.Drawing.Size(30, 23);
            this.textBox45.TabIndex = 3;
            // 
            // textBox46
            // 
            this.textBox46.Location = new System.Drawing.Point(116, 19);
            this.textBox46.Name = "textBox46";
            this.textBox46.Size = new System.Drawing.Size(30, 23);
            this.textBox46.TabIndex = 4;
            // 
            // textBox47
            // 
            this.textBox47.Location = new System.Drawing.Point(152, 19);
            this.textBox47.Name = "textBox47";
            this.textBox47.Size = new System.Drawing.Size(30, 23);
            this.textBox47.TabIndex = 5;
            // 
            // textBox48
            // 
            this.textBox48.Location = new System.Drawing.Point(188, 19);
            this.textBox48.Name = "textBox48";
            this.textBox48.Size = new System.Drawing.Size(30, 23);
            this.textBox48.TabIndex = 0;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Location = new System.Drawing.Point(51, 26);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(73, 15);
            this.label30.TabIndex = 0;
            this.label30.Text = "Short Name:";
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.textBox42);
            this.groupBox26.Controls.Add(this.textBox41);
            this.groupBox26.Controls.Add(this.textBox40);
            this.groupBox26.Controls.Add(this.textBox39);
            this.groupBox26.Controls.Add(this.textBox38);
            this.groupBox26.Controls.Add(this.textBox37);
            this.groupBox26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox26.Location = new System.Drawing.Point(14, 128);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(225, 49);
            this.groupBox26.TabIndex = 9;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Guitar String Tuning";
            // 
            // textBox42
            // 
            this.textBox42.Location = new System.Drawing.Point(188, 19);
            this.textBox42.Name = "textBox42";
            this.textBox42.Size = new System.Drawing.Size(30, 23);
            this.textBox42.TabIndex = 5;
            // 
            // textBox41
            // 
            this.textBox41.Location = new System.Drawing.Point(152, 19);
            this.textBox41.Name = "textBox41";
            this.textBox41.Size = new System.Drawing.Size(30, 23);
            this.textBox41.TabIndex = 4;
            // 
            // textBox40
            // 
            this.textBox40.Location = new System.Drawing.Point(116, 19);
            this.textBox40.Name = "textBox40";
            this.textBox40.Size = new System.Drawing.Size(30, 23);
            this.textBox40.TabIndex = 3;
            // 
            // textBox39
            // 
            this.textBox39.Location = new System.Drawing.Point(80, 19);
            this.textBox39.Name = "textBox39";
            this.textBox39.Size = new System.Drawing.Size(30, 23);
            this.textBox39.TabIndex = 2;
            // 
            // textBox38
            // 
            this.textBox38.Location = new System.Drawing.Point(44, 19);
            this.textBox38.Name = "textBox38";
            this.textBox38.Size = new System.Drawing.Size(30, 23);
            this.textBox38.TabIndex = 1;
            // 
            // textBox37
            // 
            this.textBox37.Location = new System.Drawing.Point(8, 19);
            this.textBox37.Name = "textBox37";
            this.textBox37.Size = new System.Drawing.Size(30, 23);
            this.textBox37.TabIndex = 0;
            // 
            // textBoxCONShortName
            // 
            this.textBoxCONShortName.Location = new System.Drawing.Point(130, 23);
            this.textBoxCONShortName.Name = "textBoxCONShortName";
            this.textBoxCONShortName.Size = new System.Drawing.Size(130, 23);
            this.textBoxCONShortName.TabIndex = 1;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label28.Location = new System.Drawing.Point(10, 79);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(114, 15);
            this.label28.TabIndex = 5;
            this.label28.Text = "Pro Guitar Difficulty:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(73, 52);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(51, 15);
            this.label27.TabIndex = 2;
            this.label27.Text = "Song ID:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label29.Location = new System.Drawing.Point(19, 105);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(105, 15);
            this.label29.TabIndex = 7;
            this.label29.Text = "Pro Bass Difficulty:";
            // 
            // textBoxCONSongID
            // 
            this.textBoxCONSongID.Location = new System.Drawing.Point(130, 49);
            this.textBoxCONSongID.Name = "textBoxCONSongID";
            this.textBoxCONSongID.Size = new System.Drawing.Size(104, 23);
            this.textBoxCONSongID.TabIndex = 3;
            // 
            // comboProGDifficulty
            // 
            this.comboProGDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProGDifficulty.FormattingEnabled = true;
            this.comboProGDifficulty.Items.AddRange(new object[] {
            "NO PART",
            "Warmup",
            "Apprentice",
            "Solid",
            "Moderate",
            "Challenging",
            "Nightmare",
            "Impossible"});
            this.comboProGDifficulty.Location = new System.Drawing.Point(130, 75);
            this.comboProGDifficulty.Name = "comboProGDifficulty";
            this.comboProGDifficulty.Size = new System.Drawing.Size(130, 23);
            this.comboProGDifficulty.TabIndex = 6;
            // 
            // comboProBDifficulty
            // 
            this.comboProBDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProBDifficulty.FormattingEnabled = true;
            this.comboProBDifficulty.Items.AddRange(new object[] {
            "NO PART",
            "Warmup",
            "Apprentice",
            "Solid",
            "Moderate",
            "Challenging",
            "Nightmare",
            "Impossible"});
            this.comboProBDifficulty.Location = new System.Drawing.Point(130, 101);
            this.comboProBDifficulty.Name = "comboProBDifficulty";
            this.comboProBDifficulty.Size = new System.Drawing.Size(130, 23);
            this.comboProBDifficulty.TabIndex = 8;
            // 
            // button80
            // 
            this.button80.ImageKey = "folder_explore.png";
            this.button80.ImageList = this.imageList1;
            this.button80.Location = new System.Drawing.Point(338, 62);
            this.button80.Name = "button80";
            this.button80.Size = new System.Drawing.Size(24, 24);
            this.button80.TabIndex = 4;
            this.toolTip1.SetToolTip(this.button80, "Explore File Location");
            this.button80.UseVisualStyleBackColor = true;
            this.button80.Click += new System.EventHandler(this.button80_Click);
            // 
            // button66
            // 
            this.button66.ImageIndex = 29;
            this.button66.ImageList = this.imageList1;
            this.button66.Location = new System.Drawing.Point(313, 86);
            this.button66.Name = "button66";
            this.button66.Size = new System.Drawing.Size(24, 24);
            this.button66.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button66, "Select File");
            this.button66.UseVisualStyleBackColor = true;
            this.button66.Click += new System.EventHandler(this.button66_Click);
            // 
            // button79
            // 
            this.button79.ImageKey = "folder_explore.png";
            this.button79.ImageList = this.imageList1;
            this.button79.Location = new System.Drawing.Point(338, 86);
            this.button79.Name = "button79";
            this.button79.Size = new System.Drawing.Size(24, 24);
            this.button79.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button79, "Explore File Location");
            this.button79.UseVisualStyleBackColor = true;
            this.button79.Click += new System.EventHandler(this.button79_Click);
            // 
            // button71
            // 
            this.button71.ImageIndex = 29;
            this.button71.ImageList = this.imageList1;
            this.button71.Location = new System.Drawing.Point(313, 112);
            this.button71.Name = "button71";
            this.button71.Size = new System.Drawing.Size(24, 24);
            this.button71.TabIndex = 10;
            this.toolTip1.SetToolTip(this.button71, "Select File");
            this.button71.UseVisualStyleBackColor = true;
            this.button71.Click += new System.EventHandler(this.button71_Click);
            // 
            // button78
            // 
            this.button78.ImageKey = "folder_explore.png";
            this.button78.ImageList = this.imageList1;
            this.button78.Location = new System.Drawing.Point(338, 112);
            this.button78.Name = "button78";
            this.button78.Size = new System.Drawing.Size(24, 24);
            this.button78.TabIndex = 9;
            this.toolTip1.SetToolTip(this.button78, "Explore File Location");
            this.button78.UseVisualStyleBackColor = true;
            this.button78.Click += new System.EventHandler(this.button78_Click);
            // 
            // button65
            // 
            this.button65.ImageIndex = 29;
            this.button65.ImageList = this.imageList1;
            this.button65.Location = new System.Drawing.Point(313, 62);
            this.button65.Name = "button65";
            this.button65.Size = new System.Drawing.Size(24, 24);
            this.button65.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button65, "Select File");
            this.button65.UseVisualStyleBackColor = true;
            this.button65.Click += new System.EventHandler(this.button65_Click);
            // 
            // buttonSongPropertiesSaveChanges
            // 
            this.buttonSongPropertiesSaveChanges.ImageIndex = 30;
            this.buttonSongPropertiesSaveChanges.ImageList = this.imageList1;
            this.buttonSongPropertiesSaveChanges.Location = new System.Drawing.Point(437, 269);
            this.buttonSongPropertiesSaveChanges.Name = "buttonSongPropertiesSaveChanges";
            this.buttonSongPropertiesSaveChanges.Size = new System.Drawing.Size(69, 24);
            this.buttonSongPropertiesSaveChanges.TabIndex = 19;
            this.buttonSongPropertiesSaveChanges.Text = "Save";
            this.buttonSongPropertiesSaveChanges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSongPropertiesSaveChanges.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSongPropertiesSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSongPropertiesSaveChanges.Click += new System.EventHandler(this.button67_Click);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(110, 19);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(161, 15);
            this.label37.TabIndex = 0;
            this.label37.Text = "XXXXXXXXXXXXXXXXXXXXXX";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(10, 68);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(97, 15);
            this.label34.TabIndex = 4;
            this.label34.Text = "Guitar5 Location:";
            // 
            // textBoxSongLibConFile
            // 
            this.textBoxSongLibConFile.Location = new System.Drawing.Point(113, 114);
            this.textBoxSongLibConFile.Name = "textBoxSongLibConFile";
            this.textBoxSongLibConFile.Size = new System.Drawing.Size(198, 23);
            this.textBoxSongLibConFile.TabIndex = 8;
            this.textBoxSongLibConFile.TextChanged += new System.EventHandler(this.textBox23_TextChanged);
            // 
            // textBoxSongLibProMidiFileName
            // 
            this.textBoxSongLibProMidiFileName.Location = new System.Drawing.Point(113, 89);
            this.textBoxSongLibProMidiFileName.Name = "textBoxSongLibProMidiFileName";
            this.textBoxSongLibProMidiFileName.Size = new System.Drawing.Size(198, 23);
            this.textBoxSongLibProMidiFileName.TabIndex = 5;
            this.textBoxSongLibProMidiFileName.TextChanged += new System.EventHandler(this.textBox22_TextChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(30, 94);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(77, 15);
            this.label35.TabIndex = 8;
            this.label35.Text = "Pro Location:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(24, 119);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(83, 15);
            this.label38.TabIndex = 13;
            this.label38.Text = "CON Package:";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(37, 44);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(70, 15);
            this.label39.TabIndex = 2;
            this.label39.Text = "Description:";
            // 
            // textBox24
            // 
            this.textBox24.Location = new System.Drawing.Point(113, 41);
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new System.Drawing.Size(198, 23);
            this.textBox24.TabIndex = 1;
            this.textBox24.TextChanged += new System.EventHandler(this.textBox24_TextChanged);
            // 
            // textBoxSongLibG5MidiFileName
            // 
            this.textBoxSongLibG5MidiFileName.Location = new System.Drawing.Point(113, 65);
            this.textBoxSongLibG5MidiFileName.Name = "textBoxSongLibG5MidiFileName";
            this.textBoxSongLibG5MidiFileName.Size = new System.Drawing.Size(198, 23);
            this.textBoxSongLibG5MidiFileName.TabIndex = 2;
            this.textBoxSongLibG5MidiFileName.TextChanged += new System.EventHandler(this.textBox21_TextChanged);
            // 
            // tabSongLibUtility
            // 
            this.tabSongLibUtility.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabSongLibUtility.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabSongLibUtility.Controls.Add(this.checkBoxBatchUtilExtractXMLPro);
            this.tabSongLibUtility.Controls.Add(this.checkBoxBatchUtilExtractXMLG5);
            this.tabSongLibUtility.Controls.Add(this.button94);
            this.tabSongLibUtility.Controls.Add(this.textBoxBatchUtilExtractXML);
            this.tabSongLibUtility.Controls.Add(this.checkBoxCompressAllInDefaultCONFolder);
            this.tabSongLibUtility.Controls.Add(this.groupBox4);
            this.tabSongLibUtility.Controls.Add(this.progressBarGenCompletedDifficulty);
            this.tabSongLibUtility.Controls.Add(this.button90);
            this.tabSongLibUtility.Controls.Add(this.button73);
            this.tabSongLibUtility.Controls.Add(this.button118);
            this.tabSongLibUtility.Controls.Add(this.button74);
            this.tabSongLibUtility.Controls.Add(this.textBoxCopyAllG5MidiFolder);
            this.tabSongLibUtility.Controls.Add(this.textBoxCopyAllProFolder);
            this.tabSongLibUtility.Controls.Add(this.textBoxCompressAllZipFile);
            this.tabSongLibUtility.Controls.Add(this.textBoxCopyAllCONFolder);
            this.tabSongLibUtility.Controls.Add(this.button95);
            this.tabSongLibUtility.Controls.Add(this.buttonCopyAllConToUSB);
            this.tabSongLibUtility.Controls.Add(this.button82);
            this.tabSongLibUtility.Controls.Add(this.button72);
            this.tabSongLibUtility.Controls.Add(this.button117);
            this.tabSongLibUtility.Controls.Add(this.button75);
            this.tabSongLibUtility.ImageIndex = 69;
            this.tabSongLibUtility.Location = new System.Drawing.Point(4, 23);
            this.tabSongLibUtility.Name = "tabSongLibUtility";
            this.tabSongLibUtility.Padding = new System.Windows.Forms.Padding(3);
            this.tabSongLibUtility.Size = new System.Drawing.Size(810, 489);
            this.tabSongLibUtility.TabIndex = 1;
            this.tabSongLibUtility.Text = "Batch Utilities";
            // 
            // checkBoxBatchUtilExtractXMLPro
            // 
            this.checkBoxBatchUtilExtractXMLPro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxBatchUtilExtractXMLPro.Checked = true;
            this.checkBoxBatchUtilExtractXMLPro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBatchUtilExtractXMLPro.Location = new System.Drawing.Point(532, 374);
            this.checkBoxBatchUtilExtractXMLPro.Name = "checkBoxBatchUtilExtractXMLPro";
            this.checkBoxBatchUtilExtractXMLPro.Size = new System.Drawing.Size(83, 17);
            this.checkBoxBatchUtilExtractXMLPro.TabIndex = 21;
            this.checkBoxBatchUtilExtractXMLPro.Text = "Extract Pro";
            this.checkBoxBatchUtilExtractXMLPro.UseVisualStyleBackColor = true;
            // 
            // checkBoxBatchUtilExtractXMLG5
            // 
            this.checkBoxBatchUtilExtractXMLG5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxBatchUtilExtractXMLG5.Location = new System.Drawing.Point(621, 374);
            this.checkBoxBatchUtilExtractXMLG5.Name = "checkBoxBatchUtilExtractXMLG5";
            this.checkBoxBatchUtilExtractXMLG5.Size = new System.Drawing.Size(79, 17);
            this.checkBoxBatchUtilExtractXMLG5.TabIndex = 20;
            this.checkBoxBatchUtilExtractXMLG5.Text = "Extract G5";
            this.checkBoxBatchUtilExtractXMLG5.UseVisualStyleBackColor = true;
            // 
            // button94
            // 
            this.button94.Location = new System.Drawing.Point(4, 371);
            this.button94.Name = "button94";
            this.button94.Size = new System.Drawing.Size(124, 22);
            this.button94.TabIndex = 17;
            this.button94.Text = "Extract XML:";
            this.button94.UseVisualStyleBackColor = true;
            this.button94.Click += new System.EventHandler(this.buttonBatchUtilExtractXML_Click);
            // 
            // textBoxBatchUtilExtractXML
            // 
            this.textBoxBatchUtilExtractXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBatchUtilExtractXML.Location = new System.Drawing.Point(134, 372);
            this.textBoxBatchUtilExtractXML.Name = "textBoxBatchUtilExtractXML";
            this.textBoxBatchUtilExtractXML.Size = new System.Drawing.Size(365, 23);
            this.textBoxBatchUtilExtractXML.TabIndex = 18;
            // 
            // checkBoxCompressAllInDefaultCONFolder
            // 
            this.checkBoxCompressAllInDefaultCONFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCompressAllInDefaultCONFolder.Location = new System.Drawing.Point(534, 346);
            this.checkBoxCompressAllInDefaultCONFolder.Name = "checkBoxCompressAllInDefaultCONFolder";
            this.checkBoxCompressAllInDefaultCONFolder.Size = new System.Drawing.Size(217, 17);
            this.checkBoxCompressAllInDefaultCONFolder.TabIndex = 15;
            this.checkBoxCompressAllInDefaultCONFolder.Text = "Compress All in Default CON Folder";
            this.checkBoxCompressAllInDefaultCONFolder.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox23);
            this.groupBox4.Controls.Add(this.groupBox22);
            this.groupBox4.Controls.Add(this.groupBox21);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(804, 259);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Batch Processing";
            // 
            // groupBox23
            // 
            this.groupBox23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox23.Controls.Add(this.buttonBatchOpenResult);
            this.groupBox23.Controls.Add(this.checkBatchOpenWhenCompleted);
            this.groupBox23.Controls.Add(this.textBoxSongLibBatchResults);
            this.groupBox23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox23.Location = new System.Drawing.Point(546, 20);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(252, 234);
            this.groupBox23.TabIndex = 2;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Results";
            // 
            // buttonBatchOpenResult
            // 
            this.buttonBatchOpenResult.ImageIndex = 49;
            this.buttonBatchOpenResult.ImageList = this.imageList1;
            this.buttonBatchOpenResult.Location = new System.Drawing.Point(222, 204);
            this.buttonBatchOpenResult.Name = "buttonBatchOpenResult";
            this.buttonBatchOpenResult.Size = new System.Drawing.Size(24, 24);
            this.buttonBatchOpenResult.TabIndex = 18;
            this.toolTip1.SetToolTip(this.buttonBatchOpenResult, "Open Results with Notepad");
            this.buttonBatchOpenResult.UseVisualStyleBackColor = true;
            this.buttonBatchOpenResult.Click += new System.EventHandler(this.buttonBatchOpenResult_Click);
            // 
            // checkBatchOpenWhenCompleted
            // 
            this.checkBatchOpenWhenCompleted.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBatchOpenWhenCompleted.Location = new System.Drawing.Point(6, 205);
            this.checkBatchOpenWhenCompleted.Name = "checkBatchOpenWhenCompleted";
            this.checkBatchOpenWhenCompleted.Size = new System.Drawing.Size(153, 23);
            this.checkBatchOpenWhenCompleted.TabIndex = 17;
            this.checkBatchOpenWhenCompleted.Text = "Open when completed";
            this.checkBatchOpenWhenCompleted.UseVisualStyleBackColor = true;
            // 
            // textBoxSongLibBatchResults
            // 
            this.textBoxSongLibBatchResults.AcceptsReturn = true;
            this.textBoxSongLibBatchResults.AcceptsTab = true;
            this.textBoxSongLibBatchResults.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSongLibBatchResults.Font = new System.Drawing.Font("Courier New", 10F);
            this.textBoxSongLibBatchResults.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxSongLibBatchResults.Location = new System.Drawing.Point(6, 16);
            this.textBoxSongLibBatchResults.MaxLength = 327670;
            this.textBoxSongLibBatchResults.Multiline = true;
            this.textBoxSongLibBatchResults.Name = "textBoxSongLibBatchResults";
            this.textBoxSongLibBatchResults.ReadOnly = true;
            this.textBoxSongLibBatchResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSongLibBatchResults.Size = new System.Drawing.Size(240, 182);
            this.textBoxSongLibBatchResults.TabIndex = 0;
            this.textBoxSongLibBatchResults.WordWrap = false;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.buttonBatchBuildTextEvents);
            this.groupBox22.Controls.Add(this.checkBatchGenerateTrainersIfNone);
            this.groupBox22.Controls.Add(this.checkBatchCopyTextEvents);
            this.groupBox22.Controls.Add(this.buttonExecuteBatchCopyUSB);
            this.groupBox22.Controls.Add(this.checkBoxBatchCopyUSB);
            this.groupBox22.Controls.Add(this.button106);
            this.groupBox22.Controls.Add(this.button105);
            this.groupBox22.Controls.Add(this.buttonExecuteBatchGuitarBassCopy);
            this.groupBox22.Controls.Add(this.button103);
            this.groupBox22.Controls.Add(this.checkBoxBatchCheckCON);
            this.groupBox22.Controls.Add(this.checkBoxBatchGenerateDifficulties);
            this.groupBox22.Controls.Add(this.buttonSongLibCancel);
            this.groupBox22.Controls.Add(this.button89);
            this.groupBox22.Controls.Add(this.checkBoxSkipGenIfEasyNotes);
            this.groupBox22.Controls.Add(this.checkBoxSetBassToGuitarDifficulty);
            this.groupBox22.Controls.Add(this.checkBoxBatchGuitarBassCopy);
            this.groupBox22.Controls.Add(this.checkBoxBatchRebuildCON);
            this.groupBox22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox22.Location = new System.Drawing.Point(183, 20);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(357, 233);
            this.groupBox22.TabIndex = 1;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Commands";
            // 
            // buttonBatchBuildTextEvents
            // 
            this.buttonBatchBuildTextEvents.ImageIndex = 66;
            this.buttonBatchBuildTextEvents.ImageList = this.imageList1;
            this.buttonBatchBuildTextEvents.Location = new System.Drawing.Point(6, 52);
            this.buttonBatchBuildTextEvents.Name = "buttonBatchBuildTextEvents";
            this.buttonBatchBuildTextEvents.Size = new System.Drawing.Size(24, 24);
            this.buttonBatchBuildTextEvents.TabIndex = 16;
            this.toolTip1.SetToolTip(this.buttonBatchBuildTextEvents, "Build Text Events");
            this.buttonBatchBuildTextEvents.UseVisualStyleBackColor = true;
            this.buttonBatchBuildTextEvents.Click += new System.EventHandler(this.buttonBatchBuildTextEvents_Click);
            // 
            // checkBatchGenerateTrainersIfNone
            // 
            this.checkBatchGenerateTrainersIfNone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBatchGenerateTrainersIfNone.Location = new System.Drawing.Point(51, 76);
            this.checkBatchGenerateTrainersIfNone.Name = "checkBatchGenerateTrainersIfNone";
            this.checkBatchGenerateTrainersIfNone.Size = new System.Drawing.Size(181, 17);
            this.checkBatchGenerateTrainersIfNone.TabIndex = 15;
            this.checkBatchGenerateTrainersIfNone.Text = "Generate Trainers if None";
            this.checkBatchGenerateTrainersIfNone.UseVisualStyleBackColor = true;
            // 
            // checkBatchCopyTextEvents
            // 
            this.checkBatchCopyTextEvents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBatchCopyTextEvents.Location = new System.Drawing.Point(32, 56);
            this.checkBatchCopyTextEvents.Name = "checkBatchCopyTextEvents";
            this.checkBatchCopyTextEvents.Size = new System.Drawing.Size(169, 19);
            this.checkBatchCopyTextEvents.TabIndex = 14;
            this.checkBatchCopyTextEvents.Text = "Copy Text Events if None";
            this.checkBatchCopyTextEvents.UseVisualStyleBackColor = true;
            // 
            // buttonExecuteBatchCopyUSB
            // 
            this.buttonExecuteBatchCopyUSB.ImageIndex = 21;
            this.buttonExecuteBatchCopyUSB.ImageList = this.imageList1;
            this.buttonExecuteBatchCopyUSB.Location = new System.Drawing.Point(6, 175);
            this.buttonExecuteBatchCopyUSB.Name = "buttonExecuteBatchCopyUSB";
            this.buttonExecuteBatchCopyUSB.Size = new System.Drawing.Size(24, 24);
            this.buttonExecuteBatchCopyUSB.TabIndex = 12;
            this.buttonExecuteBatchCopyUSB.UseVisualStyleBackColor = true;
            this.buttonExecuteBatchCopyUSB.Click += new System.EventHandler(this.buttonExecuteBatchCopyUSB_Click);
            // 
            // checkBoxBatchCopyUSB
            // 
            this.checkBoxBatchCopyUSB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxBatchCopyUSB.Location = new System.Drawing.Point(32, 179);
            this.checkBoxBatchCopyUSB.Name = "checkBoxBatchCopyUSB";
            this.checkBoxBatchCopyUSB.Size = new System.Drawing.Size(132, 19);
            this.checkBoxBatchCopyUSB.TabIndex = 13;
            this.checkBoxBatchCopyUSB.Text = "Copy to USB";
            this.checkBoxBatchCopyUSB.UseVisualStyleBackColor = true;
            // 
            // button106
            // 
            this.button106.ImageIndex = 16;
            this.button106.ImageList = this.imageList1;
            this.button106.Location = new System.Drawing.Point(6, 151);
            this.button106.Name = "button106";
            this.button106.Size = new System.Drawing.Size(24, 24);
            this.button106.TabIndex = 8;
            this.button106.UseVisualStyleBackColor = true;
            this.button106.Click += new System.EventHandler(this.button106_Click);
            // 
            // button105
            // 
            this.button105.ImageIndex = 37;
            this.button105.ImageList = this.imageList1;
            this.button105.Location = new System.Drawing.Point(6, 128);
            this.button105.Name = "button105";
            this.button105.Size = new System.Drawing.Size(24, 24);
            this.button105.TabIndex = 6;
            this.button105.UseVisualStyleBackColor = true;
            this.button105.Click += new System.EventHandler(this.button105_Click);
            // 
            // buttonExecuteBatchGuitarBassCopy
            // 
            this.buttonExecuteBatchGuitarBassCopy.ImageIndex = 14;
            this.buttonExecuteBatchGuitarBassCopy.ImageList = this.imageList1;
            this.buttonExecuteBatchGuitarBassCopy.Location = new System.Drawing.Point(6, 90);
            this.buttonExecuteBatchGuitarBassCopy.Name = "buttonExecuteBatchGuitarBassCopy";
            this.buttonExecuteBatchGuitarBassCopy.Size = new System.Drawing.Size(24, 24);
            this.buttonExecuteBatchGuitarBassCopy.TabIndex = 3;
            this.buttonExecuteBatchGuitarBassCopy.UseVisualStyleBackColor = true;
            this.buttonExecuteBatchGuitarBassCopy.Click += new System.EventHandler(this.buttonExecuteBatchGuitarBassCopy_Click);
            // 
            // button103
            // 
            this.button103.ImageIndex = 66;
            this.button103.ImageList = this.imageList1;
            this.button103.Location = new System.Drawing.Point(6, 16);
            this.button103.Name = "button103";
            this.button103.Size = new System.Drawing.Size(24, 24);
            this.button103.TabIndex = 0;
            this.button103.UseVisualStyleBackColor = true;
            this.button103.Click += new System.EventHandler(this.button103_Click);
            // 
            // checkBoxBatchCheckCON
            // 
            this.checkBoxBatchCheckCON.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxBatchCheckCON.Location = new System.Drawing.Point(32, 156);
            this.checkBoxBatchCheckCON.Name = "checkBoxBatchCheckCON";
            this.checkBoxBatchCheckCON.Size = new System.Drawing.Size(107, 17);
            this.checkBoxBatchCheckCON.TabIndex = 9;
            this.checkBoxBatchCheckCON.Text = "Check CON Files";
            this.checkBoxBatchCheckCON.UseVisualStyleBackColor = true;
            // 
            // checkBoxBatchGenerateDifficulties
            // 
            this.checkBoxBatchGenerateDifficulties.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxBatchGenerateDifficulties.Location = new System.Drawing.Point(32, 21);
            this.checkBoxBatchGenerateDifficulties.Name = "checkBoxBatchGenerateDifficulties";
            this.checkBoxBatchGenerateDifficulties.Size = new System.Drawing.Size(200, 17);
            this.checkBoxBatchGenerateDifficulties.TabIndex = 1;
            this.checkBoxBatchGenerateDifficulties.Text = "Generate Difficulties";
            this.checkBoxBatchGenerateDifficulties.UseVisualStyleBackColor = true;
            // 
            // buttonSongLibCancel
            // 
            this.buttonSongLibCancel.Enabled = false;
            this.buttonSongLibCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongLibCancel.ImageIndex = 31;
            this.buttonSongLibCancel.ImageList = this.imageList1;
            this.buttonSongLibCancel.Location = new System.Drawing.Point(284, 203);
            this.buttonSongLibCancel.Name = "buttonSongLibCancel";
            this.buttonSongLibCancel.Size = new System.Drawing.Size(67, 24);
            this.buttonSongLibCancel.TabIndex = 11;
            this.buttonSongLibCancel.Text = "Cancel";
            this.buttonSongLibCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSongLibCancel.UseVisualStyleBackColor = true;
            this.buttonSongLibCancel.Click += new System.EventHandler(this.buttonSongLibCancel_Click);
            // 
            // button89
            // 
            this.button89.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button89.ImageIndex = 68;
            this.button89.ImageList = this.imageList1;
            this.button89.Location = new System.Drawing.Point(209, 203);
            this.button89.Name = "button89";
            this.button89.Size = new System.Drawing.Size(74, 24);
            this.button89.TabIndex = 10;
            this.button89.Text = "Execute";
            this.button89.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button89.UseVisualStyleBackColor = true;
            this.button89.Click += new System.EventHandler(this.button89_Click);
            // 
            // checkBoxSkipGenIfEasyNotes
            // 
            this.checkBoxSkipGenIfEasyNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSkipGenIfEasyNotes.Location = new System.Drawing.Point(52, 38);
            this.checkBoxSkipGenIfEasyNotes.Name = "checkBoxSkipGenIfEasyNotes";
            this.checkBoxSkipGenIfEasyNotes.Size = new System.Drawing.Size(195, 18);
            this.checkBoxSkipGenIfEasyNotes.TabIndex = 2;
            this.checkBoxSkipGenIfEasyNotes.Text = "Skip if has easy/med/hard notes";
            this.checkBoxSkipGenIfEasyNotes.UseVisualStyleBackColor = true;
            // 
            // checkBoxSetBassToGuitarDifficulty
            // 
            this.checkBoxSetBassToGuitarDifficulty.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSetBassToGuitarDifficulty.Location = new System.Drawing.Point(52, 113);
            this.checkBoxSetBassToGuitarDifficulty.Name = "checkBoxSetBassToGuitarDifficulty";
            this.checkBoxSetBassToGuitarDifficulty.Size = new System.Drawing.Size(180, 17);
            this.checkBoxSetBassToGuitarDifficulty.TabIndex = 5;
            this.checkBoxSetBassToGuitarDifficulty.Text = "Set Bass To Same Difficulty";
            this.checkBoxSetBassToGuitarDifficulty.UseVisualStyleBackColor = true;
            // 
            // checkBoxBatchGuitarBassCopy
            // 
            this.checkBoxBatchGuitarBassCopy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxBatchGuitarBassCopy.Location = new System.Drawing.Point(32, 94);
            this.checkBoxBatchGuitarBassCopy.Name = "checkBoxBatchGuitarBassCopy";
            this.checkBoxBatchGuitarBassCopy.Size = new System.Drawing.Size(158, 19);
            this.checkBoxBatchGuitarBassCopy.TabIndex = 4;
            this.checkBoxBatchGuitarBassCopy.Text = "Copy Guitar To Bass";
            this.checkBoxBatchGuitarBassCopy.UseVisualStyleBackColor = true;
            // 
            // checkBoxBatchRebuildCON
            // 
            this.checkBoxBatchRebuildCON.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxBatchRebuildCON.Location = new System.Drawing.Point(32, 133);
            this.checkBoxBatchRebuildCON.Name = "checkBoxBatchRebuildCON";
            this.checkBoxBatchRebuildCON.Size = new System.Drawing.Size(126, 17);
            this.checkBoxBatchRebuildCON.TabIndex = 7;
            this.checkBoxBatchRebuildCON.Text = "Re-Build CON Files";
            this.checkBoxBatchRebuildCON.UseVisualStyleBackColor = true;
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.button93);
            this.groupBox21.Controls.Add(this.checkBoxBatchProcessFinalized);
            this.groupBox21.Controls.Add(this.checkBoxBatchProcessIncomplete);
            this.groupBox21.Controls.Add(this.checkBoxMultiSelectionSongList);
            this.groupBox21.Controls.Add(this.checkBoxSmokeTest);
            this.groupBox21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox21.Location = new System.Drawing.Point(6, 20);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(171, 233);
            this.groupBox21.TabIndex = 0;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Options";
            // 
            // button93
            // 
            this.button93.ImageIndex = 74;
            this.button93.ImageList = this.imageList1;
            this.button93.Location = new System.Drawing.Point(6, 203);
            this.button93.Name = "button93";
            this.button93.Size = new System.Drawing.Size(27, 24);
            this.button93.TabIndex = 17;
            this.toolTip1.SetToolTip(this.button93, "Copy Song List to Clipboard");
            this.button93.UseVisualStyleBackColor = true;
            this.button93.Click += new System.EventHandler(this.button93_Click);
            // 
            // checkBoxBatchProcessFinalized
            // 
            this.checkBoxBatchProcessFinalized.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBatchProcessFinalized.Location = new System.Drawing.Point(6, 37);
            this.checkBoxBatchProcessFinalized.Name = "checkBoxBatchProcessFinalized";
            this.checkBoxBatchProcessFinalized.Size = new System.Drawing.Size(133, 17);
            this.checkBoxBatchProcessFinalized.TabIndex = 1;
            this.checkBoxBatchProcessFinalized.Text = "Process Finalized";
            this.checkBoxBatchProcessFinalized.UseVisualStyleBackColor = true;
            this.checkBoxBatchProcessFinalized.CheckedChanged += new System.EventHandler(this.checkBoxBatchProcessFinalized_CheckedChanged);
            // 
            // checkBoxBatchProcessIncomplete
            // 
            this.checkBoxBatchProcessIncomplete.ForeColor = System.Drawing.Color.Black;
            this.checkBoxBatchProcessIncomplete.Location = new System.Drawing.Point(6, 19);
            this.checkBoxBatchProcessIncomplete.Name = "checkBoxBatchProcessIncomplete";
            this.checkBoxBatchProcessIncomplete.Size = new System.Drawing.Size(144, 17);
            this.checkBoxBatchProcessIncomplete.TabIndex = 0;
            this.checkBoxBatchProcessIncomplete.Text = "Process Incomplete";
            this.checkBoxBatchProcessIncomplete.UseVisualStyleBackColor = true;
            this.checkBoxBatchProcessIncomplete.CheckedChanged += new System.EventHandler(this.checkBoxBatchProcessIncomplete_CheckedChanged);
            this.checkBoxBatchProcessIncomplete.Click += new System.EventHandler(this.checkBoxBatchProcessIncomplete_Click);
            // 
            // checkBoxMultiSelectionSongList
            // 
            this.checkBoxMultiSelectionSongList.ForeColor = System.Drawing.Color.Black;
            this.checkBoxMultiSelectionSongList.Location = new System.Drawing.Point(6, 55);
            this.checkBoxMultiSelectionSongList.Name = "checkBoxMultiSelectionSongList";
            this.checkBoxMultiSelectionSongList.Size = new System.Drawing.Size(144, 19);
            this.checkBoxMultiSelectionSongList.TabIndex = 2;
            this.checkBoxMultiSelectionSongList.Text = "Selected Songs Only";
            this.checkBoxMultiSelectionSongList.UseVisualStyleBackColor = true;
            this.checkBoxMultiSelectionSongList.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBoxSmokeTest
            // 
            this.checkBoxSmokeTest.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSmokeTest.Location = new System.Drawing.Point(6, 76);
            this.checkBoxSmokeTest.Name = "checkBoxSmokeTest";
            this.checkBoxSmokeTest.Size = new System.Drawing.Size(159, 17);
            this.checkBoxSmokeTest.TabIndex = 3;
            this.checkBoxSmokeTest.Text = "Smoke Test (dont save)";
            this.checkBoxSmokeTest.UseVisualStyleBackColor = true;
            // 
            // progressBarGenCompletedDifficulty
            // 
            this.progressBarGenCompletedDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarGenCompletedDifficulty.Location = new System.Drawing.Point(6, 406);
            this.progressBarGenCompletedDifficulty.Name = "progressBarGenCompletedDifficulty";
            this.progressBarGenCompletedDifficulty.Size = new System.Drawing.Size(798, 12);
            this.progressBarGenCompletedDifficulty.Step = 1;
            this.progressBarGenCompletedDifficulty.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarGenCompletedDifficulty.TabIndex = 13;
            // 
            // button90
            // 
            this.button90.Location = new System.Drawing.Point(3, 266);
            this.button90.Name = "button90";
            this.button90.Size = new System.Drawing.Size(125, 22);
            this.button90.TabIndex = 0;
            this.button90.Text = "Copy All G5 MIDI To:";
            this.button90.UseVisualStyleBackColor = true;
            this.button90.Click += new System.EventHandler(this.button90_Click);
            // 
            // button73
            // 
            this.button73.Location = new System.Drawing.Point(3, 291);
            this.button73.Name = "button73";
            this.button73.Size = new System.Drawing.Size(125, 22);
            this.button73.TabIndex = 3;
            this.button73.Text = "Copy All Pro MIDI To:";
            this.button73.UseVisualStyleBackColor = true;
            this.button73.Click += new System.EventHandler(this.button73_Click);
            // 
            // button118
            // 
            this.button118.Location = new System.Drawing.Point(4, 343);
            this.button118.Name = "button118";
            this.button118.Size = new System.Drawing.Size(124, 22);
            this.button118.TabIndex = 6;
            this.button118.Text = "Compress All to Zip:";
            this.button118.UseVisualStyleBackColor = true;
            this.button118.Click += new System.EventHandler(this.button118_Click);
            // 
            // button74
            // 
            this.button74.Location = new System.Drawing.Point(4, 316);
            this.button74.Name = "button74";
            this.button74.Size = new System.Drawing.Size(124, 22);
            this.button74.TabIndex = 6;
            this.button74.Text = "Copy All CON To:";
            this.button74.UseVisualStyleBackColor = true;
            this.button74.Click += new System.EventHandler(this.button74_Click);
            // 
            // textBoxCopyAllG5MidiFolder
            // 
            this.textBoxCopyAllG5MidiFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCopyAllG5MidiFolder.Location = new System.Drawing.Point(134, 268);
            this.textBoxCopyAllG5MidiFolder.Name = "textBoxCopyAllG5MidiFolder";
            this.textBoxCopyAllG5MidiFolder.Size = new System.Drawing.Size(365, 23);
            this.textBoxCopyAllG5MidiFolder.TabIndex = 1;
            // 
            // textBoxCopyAllProFolder
            // 
            this.textBoxCopyAllProFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCopyAllProFolder.Location = new System.Drawing.Point(134, 293);
            this.textBoxCopyAllProFolder.Name = "textBoxCopyAllProFolder";
            this.textBoxCopyAllProFolder.Size = new System.Drawing.Size(365, 23);
            this.textBoxCopyAllProFolder.TabIndex = 4;
            // 
            // textBoxCompressAllZipFile
            // 
            this.textBoxCompressAllZipFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCompressAllZipFile.Location = new System.Drawing.Point(134, 344);
            this.textBoxCompressAllZipFile.Name = "textBoxCompressAllZipFile";
            this.textBoxCompressAllZipFile.Size = new System.Drawing.Size(365, 23);
            this.textBoxCompressAllZipFile.TabIndex = 7;
            // 
            // textBoxCopyAllCONFolder
            // 
            this.textBoxCopyAllCONFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCopyAllCONFolder.Location = new System.Drawing.Point(134, 318);
            this.textBoxCopyAllCONFolder.Name = "textBoxCopyAllCONFolder";
            this.textBoxCopyAllCONFolder.Size = new System.Drawing.Size(365, 23);
            this.textBoxCopyAllCONFolder.TabIndex = 7;
            // 
            // button95
            // 
            this.button95.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button95.ImageIndex = 29;
            this.button95.ImageList = this.imageList1;
            this.button95.Location = new System.Drawing.Point(504, 369);
            this.button95.Name = "button95";
            this.button95.Size = new System.Drawing.Size(24, 24);
            this.button95.TabIndex = 19;
            this.toolTip1.SetToolTip(this.button95, "Select Location");
            this.button95.UseVisualStyleBackColor = true;
            this.button95.Click += new System.EventHandler(this.buttonBatchUtilExtractXMLBrowse_Click);
            // 
            // buttonCopyAllConToUSB
            // 
            this.buttonCopyAllConToUSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyAllConToUSB.ImageIndex = 21;
            this.buttonCopyAllConToUSB.ImageList = this.imageList1;
            this.buttonCopyAllConToUSB.Location = new System.Drawing.Point(532, 316);
            this.buttonCopyAllConToUSB.Name = "buttonCopyAllConToUSB";
            this.buttonCopyAllConToUSB.Size = new System.Drawing.Size(24, 24);
            this.buttonCopyAllConToUSB.TabIndex = 16;
            this.buttonCopyAllConToUSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonCopyAllConToUSB, "Copy Packages to USB");
            this.buttonCopyAllConToUSB.UseVisualStyleBackColor = true;
            this.buttonCopyAllConToUSB.Click += new System.EventHandler(this.buttonCopyAllConToUSB_Click);
            // 
            // button82
            // 
            this.button82.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button82.ImageIndex = 29;
            this.button82.ImageList = this.imageList1;
            this.button82.Location = new System.Drawing.Point(504, 265);
            this.button82.Name = "button82";
            this.button82.Size = new System.Drawing.Size(24, 24);
            this.button82.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button82, "Select Location");
            this.button82.UseVisualStyleBackColor = true;
            this.button82.Click += new System.EventHandler(this.button82_Click);
            // 
            // button72
            // 
            this.button72.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button72.ImageIndex = 29;
            this.button72.ImageList = this.imageList1;
            this.button72.Location = new System.Drawing.Point(504, 290);
            this.button72.Name = "button72";
            this.button72.Size = new System.Drawing.Size(24, 24);
            this.button72.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button72, "Select Location");
            this.button72.UseVisualStyleBackColor = true;
            this.button72.Click += new System.EventHandler(this.button72_Click);
            // 
            // button117
            // 
            this.button117.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button117.ImageIndex = 29;
            this.button117.ImageList = this.imageList1;
            this.button117.Location = new System.Drawing.Point(504, 341);
            this.button117.Name = "button117";
            this.button117.Size = new System.Drawing.Size(24, 24);
            this.button117.TabIndex = 8;
            this.toolTip1.SetToolTip(this.button117, "Select Location");
            this.button117.UseVisualStyleBackColor = true;
            this.button117.Click += new System.EventHandler(this.button117_Click);
            // 
            // button75
            // 
            this.button75.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button75.ImageIndex = 29;
            this.button75.ImageList = this.imageList1;
            this.button75.Location = new System.Drawing.Point(504, 315);
            this.button75.Name = "button75";
            this.button75.Size = new System.Drawing.Size(24, 24);
            this.button75.TabIndex = 8;
            this.toolTip1.SetToolTip(this.button75, "Select Location");
            this.button75.UseVisualStyleBackColor = true;
            this.button75.Click += new System.EventHandler(this.button75_Click);
            // 
            // tabSongLibSongUtility
            // 
            this.tabSongLibSongUtility.Controls.Add(this.groupBox47);
            this.tabSongLibSongUtility.Location = new System.Drawing.Point(4, 23);
            this.tabSongLibSongUtility.Name = "tabSongLibSongUtility";
            this.tabSongLibSongUtility.Size = new System.Drawing.Size(810, 489);
            this.tabSongLibSongUtility.TabIndex = 2;
            this.tabSongLibSongUtility.Text = "Song Utility";
            this.tabSongLibSongUtility.UseVisualStyleBackColor = true;
            // 
            // groupBox47
            // 
            this.groupBox47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.groupBox47.Controls.Add(this.groupBox49);
            this.groupBox47.Controls.Add(this.groupBoxSongUtilFindInFile);
            this.groupBox47.Controls.Add(this.buttonSongUtilSearchFolderExplore);
            this.groupBox47.Controls.Add(this.textBoxSongUtilSearchFolder);
            this.groupBox47.Controls.Add(this.buttonSongUtilSearchForG5FromOpenPro);
            this.groupBox47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox47.Location = new System.Drawing.Point(0, 0);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Size = new System.Drawing.Size(810, 489);
            this.groupBox47.TabIndex = 0;
            this.groupBox47.TabStop = false;
            this.groupBox47.Text = "Song Utilities";
            // 
            // groupBox49
            // 
            this.groupBox49.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox49.Controls.Add(this.buttonSongUtilFindInFileResultsOpenWindow);
            this.groupBox49.Controls.Add(this.checkBoxSongUtilFindInFileResultsOpenCompleted);
            this.groupBox49.Controls.Add(this.textBoxSongUtilFindInFileResults);
            this.groupBox49.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox49.Location = new System.Drawing.Point(6, 218);
            this.groupBox49.Name = "groupBox49";
            this.groupBox49.Size = new System.Drawing.Size(798, 235);
            this.groupBox49.TabIndex = 14;
            this.groupBox49.TabStop = false;
            this.groupBox49.Text = "Results";
            // 
            // buttonSongUtilFindInFileResultsOpenWindow
            // 
            this.buttonSongUtilFindInFileResultsOpenWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSongUtilFindInFileResultsOpenWindow.ImageIndex = 49;
            this.buttonSongUtilFindInFileResultsOpenWindow.ImageList = this.imageList1;
            this.buttonSongUtilFindInFileResultsOpenWindow.Location = new System.Drawing.Point(768, 205);
            this.buttonSongUtilFindInFileResultsOpenWindow.Name = "buttonSongUtilFindInFileResultsOpenWindow";
            this.buttonSongUtilFindInFileResultsOpenWindow.Size = new System.Drawing.Size(24, 24);
            this.buttonSongUtilFindInFileResultsOpenWindow.TabIndex = 18;
            this.toolTip1.SetToolTip(this.buttonSongUtilFindInFileResultsOpenWindow, "Open Results with Notepad");
            this.buttonSongUtilFindInFileResultsOpenWindow.UseVisualStyleBackColor = true;
            this.buttonSongUtilFindInFileResultsOpenWindow.Click += new System.EventHandler(this.buttonSongUtilFindInFileResultsOpenWindow_Click);
            // 
            // checkBoxSongUtilFindInFileResultsOpenCompleted
            // 
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.Location = new System.Drawing.Point(6, 206);
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.Name = "checkBoxSongUtilFindInFileResultsOpenCompleted";
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.Size = new System.Drawing.Size(153, 23);
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.TabIndex = 17;
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.Text = "Open when completed";
            this.checkBoxSongUtilFindInFileResultsOpenCompleted.UseVisualStyleBackColor = true;
            // 
            // textBoxSongUtilFindInFileResults
            // 
            this.textBoxSongUtilFindInFileResults.AcceptsReturn = true;
            this.textBoxSongUtilFindInFileResults.AcceptsTab = true;
            this.textBoxSongUtilFindInFileResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSongUtilFindInFileResults.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSongUtilFindInFileResults.Font = new System.Drawing.Font("Courier New", 10F);
            this.textBoxSongUtilFindInFileResults.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxSongUtilFindInFileResults.Location = new System.Drawing.Point(6, 16);
            this.textBoxSongUtilFindInFileResults.MaxLength = 327670;
            this.textBoxSongUtilFindInFileResults.Multiline = true;
            this.textBoxSongUtilFindInFileResults.Name = "textBoxSongUtilFindInFileResults";
            this.textBoxSongUtilFindInFileResults.ReadOnly = true;
            this.textBoxSongUtilFindInFileResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSongUtilFindInFileResults.Size = new System.Drawing.Size(786, 183);
            this.textBoxSongUtilFindInFileResults.TabIndex = 0;
            this.textBoxSongUtilFindInFileResults.WordWrap = false;
            // 
            // groupBoxSongUtilFindInFile
            // 
            this.groupBoxSongUtilFindInFile.Controls.Add(this.textBoxSongUtilFindFolder);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.checkBoxSongUtilFindInProOnly);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.label60);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.buttonSongUtilFindFolder);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.buttonSongUtilFindInFileDistinctText);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.checkBoxSongUtilFindInFileMatchWholeWord);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.checkBoxSongUtilFindInFileMatchCountOnly);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.checkBoxSongUtilFindInFileFirstMatchOnly);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.checkBoxSongUtilFindInFileSelectedSongOnly);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.textBoxSongUtilFindInFileChan);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.textBoxSongUtilFindInFileText);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.textBoxSongUtilFindInFileData1);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.textBoxSongUtilFindInFileData2);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.label59);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.buttonSongUtilFindInFileSearch);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.label58);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.label56);
            this.groupBoxSongUtilFindInFile.Controls.Add(this.label57);
            this.groupBoxSongUtilFindInFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxSongUtilFindInFile.Location = new System.Drawing.Point(6, 52);
            this.groupBoxSongUtilFindInFile.Name = "groupBoxSongUtilFindInFile";
            this.groupBoxSongUtilFindInFile.Size = new System.Drawing.Size(701, 160);
            this.groupBoxSongUtilFindInFile.TabIndex = 0;
            this.groupBoxSongUtilFindInFile.TabStop = false;
            this.groupBoxSongUtilFindInFile.Text = "Find";
            // 
            // textBoxSongUtilFindFolder
            // 
            this.textBoxSongUtilFindFolder.Location = new System.Drawing.Point(317, 36);
            this.textBoxSongUtilFindFolder.Name = "textBoxSongUtilFindFolder";
            this.textBoxSongUtilFindFolder.Size = new System.Drawing.Size(209, 23);
            this.textBoxSongUtilFindFolder.TabIndex = 14;
            // 
            // checkBoxSongUtilFindInProOnly
            // 
            this.checkBoxSongUtilFindInProOnly.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongUtilFindInProOnly.Location = new System.Drawing.Point(139, 85);
            this.checkBoxSongUtilFindInProOnly.Name = "checkBoxSongUtilFindInProOnly";
            this.checkBoxSongUtilFindInProOnly.Size = new System.Drawing.Size(150, 19);
            this.checkBoxSongUtilFindInProOnly.TabIndex = 17;
            this.checkBoxSongUtilFindInProOnly.Text = "Find In Pro Only";
            this.checkBoxSongUtilFindInProOnly.UseVisualStyleBackColor = true;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.ForeColor = System.Drawing.Color.Black;
            this.label60.Location = new System.Drawing.Point(314, 19);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(81, 15);
            this.label60.TabIndex = 16;
            this.label60.Text = "Search Folder:";
            // 
            // buttonSongUtilFindFolder
            // 
            this.buttonSongUtilFindFolder.ImageKey = "folder_explore.png";
            this.buttonSongUtilFindFolder.ImageList = this.imageList1;
            this.buttonSongUtilFindFolder.Location = new System.Drawing.Point(532, 35);
            this.buttonSongUtilFindFolder.Name = "buttonSongUtilFindFolder";
            this.buttonSongUtilFindFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonSongUtilFindFolder.TabIndex = 15;
            this.toolTip1.SetToolTip(this.buttonSongUtilFindFolder, "Explore File Location");
            this.buttonSongUtilFindFolder.UseVisualStyleBackColor = true;
            this.buttonSongUtilFindFolder.Click += new System.EventHandler(this.buttonSongUtilFindFolder_Click);
            // 
            // buttonSongUtilFindInFileDistinctText
            // 
            this.buttonSongUtilFindInFileDistinctText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongUtilFindInFileDistinctText.ImageIndex = 68;
            this.buttonSongUtilFindInFileDistinctText.ImageList = this.imageList1;
            this.buttonSongUtilFindInFileDistinctText.Location = new System.Drawing.Point(314, 129);
            this.buttonSongUtilFindInFileDistinctText.Name = "buttonSongUtilFindInFileDistinctText";
            this.buttonSongUtilFindInFileDistinctText.Size = new System.Drawing.Size(161, 24);
            this.buttonSongUtilFindInFileDistinctText.TabIndex = 12;
            this.buttonSongUtilFindInFileDistinctText.Text = "Distinct Text Events";
            this.buttonSongUtilFindInFileDistinctText.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSongUtilFindInFileDistinctText.UseVisualStyleBackColor = true;
            this.buttonSongUtilFindInFileDistinctText.Click += new System.EventHandler(this.buttonSongUtilFindInFileDistinctText_Click);
            // 
            // checkBoxSongUtilFindInFileMatchWholeWord
            // 
            this.checkBoxSongUtilFindInFileMatchWholeWord.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongUtilFindInFileMatchWholeWord.Location = new System.Drawing.Point(139, 66);
            this.checkBoxSongUtilFindInFileMatchWholeWord.Name = "checkBoxSongUtilFindInFileMatchWholeWord";
            this.checkBoxSongUtilFindInFileMatchWholeWord.Size = new System.Drawing.Size(150, 19);
            this.checkBoxSongUtilFindInFileMatchWholeWord.TabIndex = 11;
            this.checkBoxSongUtilFindInFileMatchWholeWord.Text = "Match Whole Word";
            this.checkBoxSongUtilFindInFileMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxSongUtilFindInFileMatchCountOnly
            // 
            this.checkBoxSongUtilFindInFileMatchCountOnly.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongUtilFindInFileMatchCountOnly.Location = new System.Drawing.Point(6, 134);
            this.checkBoxSongUtilFindInFileMatchCountOnly.Name = "checkBoxSongUtilFindInFileMatchCountOnly";
            this.checkBoxSongUtilFindInFileMatchCountOnly.Size = new System.Drawing.Size(150, 19);
            this.checkBoxSongUtilFindInFileMatchCountOnly.TabIndex = 10;
            this.checkBoxSongUtilFindInFileMatchCountOnly.Text = "Match Count Only";
            this.checkBoxSongUtilFindInFileMatchCountOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxSongUtilFindInFileFirstMatchOnly
            // 
            this.checkBoxSongUtilFindInFileFirstMatchOnly.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongUtilFindInFileFirstMatchOnly.Location = new System.Drawing.Point(9, 85);
            this.checkBoxSongUtilFindInFileFirstMatchOnly.Name = "checkBoxSongUtilFindInFileFirstMatchOnly";
            this.checkBoxSongUtilFindInFileFirstMatchOnly.Size = new System.Drawing.Size(124, 19);
            this.checkBoxSongUtilFindInFileFirstMatchOnly.TabIndex = 10;
            this.checkBoxSongUtilFindInFileFirstMatchOnly.Text = "First Match Only";
            this.checkBoxSongUtilFindInFileFirstMatchOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxSongUtilFindInFileSelectedSongOnly
            // 
            this.checkBoxSongUtilFindInFileSelectedSongOnly.Checked = true;
            this.checkBoxSongUtilFindInFileSelectedSongOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSongUtilFindInFileSelectedSongOnly.ForeColor = System.Drawing.Color.Black;
            this.checkBoxSongUtilFindInFileSelectedSongOnly.Location = new System.Drawing.Point(9, 66);
            this.checkBoxSongUtilFindInFileSelectedSongOnly.Name = "checkBoxSongUtilFindInFileSelectedSongOnly";
            this.checkBoxSongUtilFindInFileSelectedSongOnly.Size = new System.Drawing.Size(150, 19);
            this.checkBoxSongUtilFindInFileSelectedSongOnly.TabIndex = 10;
            this.checkBoxSongUtilFindInFileSelectedSongOnly.Text = "Selected Song Only";
            this.checkBoxSongUtilFindInFileSelectedSongOnly.UseVisualStyleBackColor = true;
            // 
            // textBoxSongUtilFindInFileChan
            // 
            this.textBoxSongUtilFindInFileChan.Location = new System.Drawing.Point(95, 36);
            this.textBoxSongUtilFindInFileChan.Name = "textBoxSongUtilFindInFileChan";
            this.textBoxSongUtilFindInFileChan.Size = new System.Drawing.Size(37, 23);
            this.textBoxSongUtilFindInFileChan.TabIndex = 5;
            // 
            // textBoxSongUtilFindInFileText
            // 
            this.textBoxSongUtilFindInFileText.Location = new System.Drawing.Point(139, 36);
            this.textBoxSongUtilFindInFileText.Name = "textBoxSongUtilFindInFileText";
            this.textBoxSongUtilFindInFileText.Size = new System.Drawing.Size(172, 23);
            this.textBoxSongUtilFindInFileText.TabIndex = 8;
            // 
            // textBoxSongUtilFindInFileData1
            // 
            this.textBoxSongUtilFindInFileData1.Location = new System.Drawing.Point(9, 37);
            this.textBoxSongUtilFindInFileData1.Name = "textBoxSongUtilFindInFileData1";
            this.textBoxSongUtilFindInFileData1.Size = new System.Drawing.Size(37, 23);
            this.textBoxSongUtilFindInFileData1.TabIndex = 1;
            // 
            // textBoxSongUtilFindInFileData2
            // 
            this.textBoxSongUtilFindInFileData2.Location = new System.Drawing.Point(52, 37);
            this.textBoxSongUtilFindInFileData2.Name = "textBoxSongUtilFindInFileData2";
            this.textBoxSongUtilFindInFileData2.Size = new System.Drawing.Size(37, 23);
            this.textBoxSongUtilFindInFileData2.TabIndex = 3;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.ForeColor = System.Drawing.Color.Black;
            this.label59.Location = new System.Drawing.Point(92, 18);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(38, 15);
            this.label59.TabIndex = 4;
            this.label59.Text = "Chan:";
            // 
            // buttonSongUtilFindInFileSearch
            // 
            this.buttonSongUtilFindInFileSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSongUtilFindInFileSearch.ImageIndex = 29;
            this.buttonSongUtilFindInFileSearch.ImageList = this.imageList1;
            this.buttonSongUtilFindInFileSearch.Location = new System.Drawing.Point(532, 129);
            this.buttonSongUtilFindInFileSearch.Name = "buttonSongUtilFindInFileSearch";
            this.buttonSongUtilFindInFileSearch.Size = new System.Drawing.Size(24, 24);
            this.buttonSongUtilFindInFileSearch.TabIndex = 9;
            this.toolTip1.SetToolTip(this.buttonSongUtilFindInFileSearch, "Find Match in Files");
            this.buttonSongUtilFindInFileSearch.UseVisualStyleBackColor = true;
            this.buttonSongUtilFindInFileSearch.Click += new System.EventHandler(this.buttonSongUtilFindInFileSearch_Click);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.ForeColor = System.Drawing.Color.Black;
            this.label58.Location = new System.Drawing.Point(136, 18);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(32, 15);
            this.label58.TabIndex = 7;
            this.label58.Text = "Text:";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.ForeColor = System.Drawing.Color.Black;
            this.label56.Location = new System.Drawing.Point(6, 19);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(40, 15);
            this.label56.TabIndex = 0;
            this.label56.Text = "Data1:";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.ForeColor = System.Drawing.Color.Black;
            this.label57.Location = new System.Drawing.Point(49, 19);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(40, 15);
            this.label57.TabIndex = 2;
            this.label57.Text = "Data2:";
            // 
            // buttonSongUtilSearchFolderExplore
            // 
            this.buttonSongUtilSearchFolderExplore.ImageKey = "folder_explore.png";
            this.buttonSongUtilSearchFolderExplore.ImageList = this.imageList1;
            this.buttonSongUtilSearchFolderExplore.Location = new System.Drawing.Point(221, 22);
            this.buttonSongUtilSearchFolderExplore.Name = "buttonSongUtilSearchFolderExplore";
            this.buttonSongUtilSearchFolderExplore.Size = new System.Drawing.Size(24, 24);
            this.buttonSongUtilSearchFolderExplore.TabIndex = 13;
            this.toolTip1.SetToolTip(this.buttonSongUtilSearchFolderExplore, "Explore File Location");
            this.buttonSongUtilSearchFolderExplore.UseVisualStyleBackColor = true;
            this.buttonSongUtilSearchFolderExplore.Click += new System.EventHandler(this.buttonSongUtilSearchFolderExplore_Click);
            // 
            // textBoxSongUtilSearchFolder
            // 
            this.textBoxSongUtilSearchFolder.Location = new System.Drawing.Point(6, 23);
            this.textBoxSongUtilSearchFolder.Name = "textBoxSongUtilSearchFolder";
            this.textBoxSongUtilSearchFolder.Size = new System.Drawing.Size(209, 23);
            this.textBoxSongUtilSearchFolder.TabIndex = 12;
            // 
            // buttonSongUtilSearchForG5FromOpenPro
            // 
            this.buttonSongUtilSearchForG5FromOpenPro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSongUtilSearchForG5FromOpenPro.ImageIndex = 68;
            this.buttonSongUtilSearchForG5FromOpenPro.ImageList = this.imageList1;
            this.buttonSongUtilSearchForG5FromOpenPro.Location = new System.Drawing.Point(251, 21);
            this.buttonSongUtilSearchForG5FromOpenPro.Name = "buttonSongUtilSearchForG5FromOpenPro";
            this.buttonSongUtilSearchForG5FromOpenPro.Size = new System.Drawing.Size(239, 24);
            this.buttonSongUtilSearchForG5FromOpenPro.TabIndex = 11;
            this.buttonSongUtilSearchForG5FromOpenPro.Text = "Search For G5 For Selected Songs";
            this.buttonSongUtilSearchForG5FromOpenPro.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSongUtilSearchForG5FromOpenPro.UseVisualStyleBackColor = true;
            this.buttonSongUtilSearchForG5FromOpenPro.Visible = false;
            this.buttonSongUtilSearchForG5FromOpenPro.Click += new System.EventHandler(this.buttonSongUtilSearchForG5FromOpenPro_Click);
            // 
            // tabTrackEditor
            // 
            this.tabTrackEditor.AutoScroll = true;
            this.tabTrackEditor.AutoScrollMinSize = new System.Drawing.Size(1010, 522);
            this.tabTrackEditor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabTrackEditor.Controls.Add(this.panel2);
            this.tabTrackEditor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabTrackEditor.ImageIndex = 46;
            this.tabTrackEditor.Location = new System.Drawing.Point(4, 23);
            this.tabTrackEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tabTrackEditor.Name = "tabTrackEditor";
            this.tabTrackEditor.Size = new System.Drawing.Size(1095, 523);
            this.tabTrackEditor.TabIndex = 1;
            this.tabTrackEditor.Text = "Track Editor";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.groupBox41);
            this.panel2.Controls.Add(groupBox24);
            this.panel2.Controls.Add(groupBox15);
            this.panel2.Controls.Add(groupBox14);
            this.panel2.Controls.Add(groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1095, 522);
            this.panel2.TabIndex = 23;
            // 
            // groupBox41
            // 
            this.groupBox41.Controls.Add(this.button131);
            this.groupBox41.Controls.Add(this.textBoxTempoDenominator);
            this.groupBox41.Controls.Add(this.textBoxTempoNumerator);
            this.groupBox41.Controls.Add(this.label3);
            this.groupBox41.Controls.Add(this.label2);
            this.groupBox41.Controls.Add(this.button130);
            this.groupBox41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox41.Location = new System.Drawing.Point(831, 117);
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.Size = new System.Drawing.Size(219, 82);
            this.groupBox41.TabIndex = 11;
            this.groupBox41.TabStop = false;
            this.groupBox41.Text = "Time Signature";
            // 
            // button131
            // 
            this.button131.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button131.ImageIndex = 36;
            this.button131.ImageList = this.imageList1;
            this.button131.Location = new System.Drawing.Point(189, 16);
            this.button131.Name = "button131";
            this.button131.Size = new System.Drawing.Size(24, 24);
            this.button131.TabIndex = 17;
            this.toolTip1.SetToolTip(this.button131, "Reset to default values");
            this.button131.UseVisualStyleBackColor = true;
            this.button131.Click += new System.EventHandler(this.button131_Click);
            // 
            // textBoxTempoDenominator
            // 
            this.textBoxTempoDenominator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxTempoDenominator.Location = new System.Drawing.Point(86, 45);
            this.textBoxTempoDenominator.Name = "textBoxTempoDenominator";
            this.textBoxTempoDenominator.Size = new System.Drawing.Size(98, 23);
            this.textBoxTempoDenominator.TabIndex = 16;
            // 
            // textBoxTempoNumerator
            // 
            this.textBoxTempoNumerator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxTempoNumerator.Location = new System.Drawing.Point(86, 19);
            this.textBoxTempoNumerator.Name = "textBoxTempoNumerator";
            this.textBoxTempoNumerator.Size = new System.Drawing.Size(98, 23);
            this.textBoxTempoNumerator.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Denominator:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(19, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Numerator:";
            // 
            // button130
            // 
            this.button130.BackColor = System.Drawing.Color.Transparent;
            this.button130.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button130.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button130.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button130.ImageIndex = 27;
            this.button130.ImageList = this.imageList1;
            this.button130.Location = new System.Drawing.Point(189, 45);
            this.button130.Name = "button130";
            this.button130.Size = new System.Drawing.Size(24, 24);
            this.button130.TabIndex = 12;
            this.button130.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button130, "Call Init Tempo if the pro track doesnt line up to the 5 button track");
            this.button130.UseVisualStyleBackColor = false;
            this.button130.Click += new System.EventHandler(this.button130_Click_1);
            // 
            // tabNoteEditor
            // 
            this.tabNoteEditor.AutoScroll = true;
            this.tabNoteEditor.AutoScrollMinSize = new System.Drawing.Size(1010, 408);
            this.tabNoteEditor.Controls.Add(this.groupBox44);
            this.tabNoteEditor.Controls.Add(this.groupBox42);
            this.tabNoteEditor.Controls.Add(this.groupBox40);
            this.tabNoteEditor.Controls.Add(this.groupBox37);
            this.tabNoteEditor.Controls.Add(this.groupBox36);
            this.tabNoteEditor.Controls.Add(this.groupBox34);
            this.tabNoteEditor.Controls.Add(this.groupBox29);
            this.tabNoteEditor.Controls.Add(this.groupBoxMidiInstrument);
            this.tabNoteEditor.Controls.Add(this.groupBox13);
            this.tabNoteEditor.Controls.Add(this.groupBox12);
            this.tabNoteEditor.Controls.Add(this.groupBox11);
            this.tabNoteEditor.Controls.Add(this.groupBox10);
            this.tabNoteEditor.Controls.Add(this.groupBox9);
            this.tabNoteEditor.Controls.Add(this.groupBox8);
            this.tabNoteEditor.Controls.Add(this.groupBox7);
            this.tabNoteEditor.Controls.Add(this.groupBox6);
            this.tabNoteEditor.Controls.Add(this.groupBox5);
            this.tabNoteEditor.Controls.Add(this.groupBox2);
            this.tabNoteEditor.Controls.Add(this.groupBoxStrumMarkers);
            this.tabNoteEditor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabNoteEditor.ImageIndex = 13;
            this.tabNoteEditor.Location = new System.Drawing.Point(4, 23);
            this.tabNoteEditor.Name = "tabNoteEditor";
            this.tabNoteEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabNoteEditor.Size = new System.Drawing.Size(1095, 523);
            this.tabNoteEditor.TabIndex = 2;
            this.tabNoteEditor.Text = "Note Editor";
            this.tabNoteEditor.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // groupBox44
            // 
            this.groupBox44.BackColor = System.Drawing.Color.Transparent;
            this.groupBox44.Controls.Add(this.comboBoxNoteEditorChordName);
            this.groupBox44.Controls.Add(this.label61);
            this.groupBox44.Controls.Add(this.checkChordNameEb);
            this.groupBox44.Controls.Add(this.checkChordNameD);
            this.groupBox44.Controls.Add(this.checkChordNameDb);
            this.groupBox44.Controls.Add(this.checkChordNameC);
            this.groupBox44.Controls.Add(this.checkChordNameG);
            this.groupBox44.Controls.Add(this.checkChordNameGb);
            this.groupBox44.Controls.Add(this.checkChordNameB);
            this.groupBox44.Controls.Add(this.checkChordNameBb);
            this.groupBox44.Controls.Add(this.checkChordNameA);
            this.groupBox44.Controls.Add(this.checkChordNameAb);
            this.groupBox44.Controls.Add(this.checkChordNameF);
            this.groupBox44.Controls.Add(this.checkChordNameE);
            this.groupBox44.Controls.Add(this.checkChordNameSlash);
            this.groupBox44.Controls.Add(this.checkChordNameHide);
            this.groupBox44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox44.Location = new System.Drawing.Point(8, 347);
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.Size = new System.Drawing.Size(197, 169);
            this.groupBox44.TabIndex = 19;
            this.groupBox44.TabStop = false;
            this.groupBox44.Text = "Chord Naming";
            // 
            // comboBoxNoteEditorChordName
            // 
            this.comboBoxNoteEditorChordName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxNoteEditorChordName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxNoteEditorChordName.FormattingEnabled = true;
            this.comboBoxNoteEditorChordName.Location = new System.Drawing.Point(9, 129);
            this.comboBoxNoteEditorChordName.Name = "comboBoxNoteEditorChordName";
            this.comboBoxNoteEditorChordName.Size = new System.Drawing.Size(176, 23);
            this.comboBoxNoteEditorChordName.TabIndex = 16;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(6, 111);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(78, 15);
            this.label61.TabIndex = 15;
            this.label61.Text = "Chord Name:";
            // 
            // checkChordNameEb
            // 
            this.checkChordNameEb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameEb.Location = new System.Drawing.Point(134, 91);
            this.checkChordNameEb.Name = "checkChordNameEb";
            this.checkChordNameEb.Size = new System.Drawing.Size(42, 17);
            this.checkChordNameEb.TabIndex = 14;
            this.checkChordNameEb.Text = "Eb";
            this.checkChordNameEb.UseVisualStyleBackColor = true;
            // 
            // checkChordNameD
            // 
            this.checkChordNameD.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameD.Location = new System.Drawing.Point(134, 74);
            this.checkChordNameD.Name = "checkChordNameD";
            this.checkChordNameD.Size = new System.Drawing.Size(37, 17);
            this.checkChordNameD.TabIndex = 13;
            this.checkChordNameD.Text = "D";
            this.checkChordNameD.UseVisualStyleBackColor = true;
            // 
            // checkChordNameDb
            // 
            this.checkChordNameDb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameDb.Location = new System.Drawing.Point(134, 56);
            this.checkChordNameDb.Name = "checkChordNameDb";
            this.checkChordNameDb.Size = new System.Drawing.Size(43, 17);
            this.checkChordNameDb.TabIndex = 12;
            this.checkChordNameDb.Text = "Db";
            this.checkChordNameDb.UseVisualStyleBackColor = true;
            // 
            // checkChordNameC
            // 
            this.checkChordNameC.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameC.Location = new System.Drawing.Point(93, 91);
            this.checkChordNameC.Name = "checkChordNameC";
            this.checkChordNameC.Size = new System.Drawing.Size(36, 17);
            this.checkChordNameC.TabIndex = 11;
            this.checkChordNameC.Text = "C";
            this.checkChordNameC.UseVisualStyleBackColor = true;
            // 
            // checkChordNameG
            // 
            this.checkChordNameG.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameG.Location = new System.Drawing.Point(50, 56);
            this.checkChordNameG.Name = "checkChordNameG";
            this.checkChordNameG.Size = new System.Drawing.Size(37, 17);
            this.checkChordNameG.TabIndex = 10;
            this.checkChordNameG.Text = "G";
            this.checkChordNameG.UseVisualStyleBackColor = true;
            // 
            // checkChordNameGb
            // 
            this.checkChordNameGb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameGb.Location = new System.Drawing.Point(5, 91);
            this.checkChordNameGb.Name = "checkChordNameGb";
            this.checkChordNameGb.Size = new System.Drawing.Size(43, 17);
            this.checkChordNameGb.TabIndex = 9;
            this.checkChordNameGb.Text = "Gb";
            this.checkChordNameGb.UseVisualStyleBackColor = true;
            // 
            // checkChordNameB
            // 
            this.checkChordNameB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameB.Location = new System.Drawing.Point(93, 74);
            this.checkChordNameB.Name = "checkChordNameB";
            this.checkChordNameB.Size = new System.Drawing.Size(35, 17);
            this.checkChordNameB.TabIndex = 8;
            this.checkChordNameB.Text = "B";
            this.checkChordNameB.UseVisualStyleBackColor = true;
            // 
            // checkChordNameBb
            // 
            this.checkChordNameBb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameBb.Location = new System.Drawing.Point(93, 56);
            this.checkChordNameBb.Name = "checkChordNameBb";
            this.checkChordNameBb.Size = new System.Drawing.Size(41, 17);
            this.checkChordNameBb.TabIndex = 7;
            this.checkChordNameBb.Text = "Bb";
            this.checkChordNameBb.UseVisualStyleBackColor = true;
            // 
            // checkChordNameA
            // 
            this.checkChordNameA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameA.Location = new System.Drawing.Point(50, 91);
            this.checkChordNameA.Name = "checkChordNameA";
            this.checkChordNameA.Size = new System.Drawing.Size(35, 17);
            this.checkChordNameA.TabIndex = 6;
            this.checkChordNameA.Text = "A";
            this.checkChordNameA.UseVisualStyleBackColor = true;
            // 
            // checkChordNameAb
            // 
            this.checkChordNameAb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameAb.Location = new System.Drawing.Point(50, 74);
            this.checkChordNameAb.Name = "checkChordNameAb";
            this.checkChordNameAb.Size = new System.Drawing.Size(41, 17);
            this.checkChordNameAb.TabIndex = 5;
            this.checkChordNameAb.Text = "Ab";
            this.checkChordNameAb.UseVisualStyleBackColor = true;
            // 
            // checkChordNameF
            // 
            this.checkChordNameF.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameF.Location = new System.Drawing.Point(5, 74);
            this.checkChordNameF.Name = "checkChordNameF";
            this.checkChordNameF.Size = new System.Drawing.Size(35, 17);
            this.checkChordNameF.TabIndex = 4;
            this.checkChordNameF.Text = "F";
            this.checkChordNameF.UseVisualStyleBackColor = true;
            // 
            // checkChordNameE
            // 
            this.checkChordNameE.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameE.Location = new System.Drawing.Point(5, 56);
            this.checkChordNameE.Name = "checkChordNameE";
            this.checkChordNameE.Size = new System.Drawing.Size(36, 17);
            this.checkChordNameE.TabIndex = 3;
            this.checkChordNameE.Text = "E";
            this.checkChordNameE.UseVisualStyleBackColor = true;
            // 
            // checkChordNameSlash
            // 
            this.checkChordNameSlash.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameSlash.Location = new System.Drawing.Point(5, 39);
            this.checkChordNameSlash.Name = "checkChordNameSlash";
            this.checkChordNameSlash.Size = new System.Drawing.Size(111, 17);
            this.checkChordNameSlash.TabIndex = 2;
            this.checkChordNameSlash.Text = "Slash Chord";
            this.checkChordNameSlash.UseVisualStyleBackColor = true;
            // 
            // checkChordNameHide
            // 
            this.checkChordNameHide.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordNameHide.Location = new System.Drawing.Point(5, 22);
            this.checkChordNameHide.Name = "checkChordNameHide";
            this.checkChordNameHide.Size = new System.Drawing.Size(138, 17);
            this.checkChordNameHide.TabIndex = 1;
            this.checkChordNameHide.Text = "Hide Chord Names";
            this.checkChordNameHide.UseVisualStyleBackColor = true;
            this.checkChordNameHide.CheckedChanged += new System.EventHandler(this.checkChordNameHide_CheckedChanged);
            // 
            // groupBox42
            // 
            this.groupBox42.Controls.Add(this.button99);
            this.groupBox42.Controls.Add(this.button100);
            this.groupBox42.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox42.Location = new System.Drawing.Point(753, 6);
            this.groupBox42.Name = "groupBox42";
            this.groupBox42.Size = new System.Drawing.Size(68, 52);
            this.groupBox42.TabIndex = 18;
            this.groupBox42.TabStop = false;
            this.groupBox42.Text = "View";
            // 
            // button99
            // 
            this.button99.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button99.ImageIndex = 85;
            this.button99.ImageList = this.imageList1;
            this.button99.Location = new System.Drawing.Point(8, 19);
            this.button99.Name = "button99";
            this.button99.Size = new System.Drawing.Size(24, 24);
            this.button99.TabIndex = 16;
            this.toolTip1.SetToolTip(this.button99, "Zoom In");
            this.button99.UseVisualStyleBackColor = true;
            this.button99.Click += new System.EventHandler(this.button99_Click_1);
            // 
            // button100
            // 
            this.button100.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button100.ImageIndex = 86;
            this.button100.ImageList = this.imageList1;
            this.button100.Location = new System.Drawing.Point(33, 19);
            this.button100.Name = "button100";
            this.button100.Size = new System.Drawing.Size(24, 24);
            this.button100.TabIndex = 17;
            this.toolTip1.SetToolTip(this.button100, "Zoom Out");
            this.button100.UseVisualStyleBackColor = true;
            this.button100.Click += new System.EventHandler(this.button100_Click_1);
            // 
            // groupBox40
            // 
            this.groupBox40.Controls.Add(this.radioNoteEditDifficultyExpert);
            this.groupBox40.Controls.Add(this.radioNoteEditDifficultyHard);
            this.groupBox40.Controls.Add(this.radioNoteEditDifficultyMedium);
            this.groupBox40.Controls.Add(this.radioNoteEditDifficultyEasy);
            this.groupBox40.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox40.Location = new System.Drawing.Point(345, 152);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Size = new System.Drawing.Size(82, 115);
            this.groupBox40.TabIndex = 15;
            this.groupBox40.TabStop = false;
            this.groupBox40.Text = "Difficulty";
            // 
            // radioNoteEditDifficultyExpert
            // 
            this.radioNoteEditDifficultyExpert.Checked = true;
            this.radioNoteEditDifficultyExpert.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioNoteEditDifficultyExpert.Location = new System.Drawing.Point(6, 79);
            this.radioNoteEditDifficultyExpert.Name = "radioNoteEditDifficultyExpert";
            this.radioNoteEditDifficultyExpert.Size = new System.Drawing.Size(70, 17);
            this.radioNoteEditDifficultyExpert.TabIndex = 7;
            this.radioNoteEditDifficultyExpert.TabStop = true;
            this.radioNoteEditDifficultyExpert.Text = "Expert";
            this.radioNoteEditDifficultyExpert.UseCompatibleTextRendering = true;
            this.radioNoteEditDifficultyExpert.UseVisualStyleBackColor = true;
            this.radioNoteEditDifficultyExpert.Click += new System.EventHandler(this.radioNoteEditDifficultyExpert_Click);
            // 
            // radioNoteEditDifficultyHard
            // 
            this.radioNoteEditDifficultyHard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioNoteEditDifficultyHard.Location = new System.Drawing.Point(6, 60);
            this.radioNoteEditDifficultyHard.Name = "radioNoteEditDifficultyHard";
            this.radioNoteEditDifficultyHard.Size = new System.Drawing.Size(70, 17);
            this.radioNoteEditDifficultyHard.TabIndex = 6;
            this.radioNoteEditDifficultyHard.Text = "Hard";
            this.radioNoteEditDifficultyHard.UseCompatibleTextRendering = true;
            this.radioNoteEditDifficultyHard.UseVisualStyleBackColor = true;
            this.radioNoteEditDifficultyHard.Click += new System.EventHandler(this.radioNoteEditDifficultyHard_Click);
            // 
            // radioNoteEditDifficultyMedium
            // 
            this.radioNoteEditDifficultyMedium.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioNoteEditDifficultyMedium.Location = new System.Drawing.Point(6, 41);
            this.radioNoteEditDifficultyMedium.Name = "radioNoteEditDifficultyMedium";
            this.radioNoteEditDifficultyMedium.Size = new System.Drawing.Size(70, 17);
            this.radioNoteEditDifficultyMedium.TabIndex = 5;
            this.radioNoteEditDifficultyMedium.Text = "Medium";
            this.radioNoteEditDifficultyMedium.UseCompatibleTextRendering = true;
            this.radioNoteEditDifficultyMedium.UseVisualStyleBackColor = true;
            this.radioNoteEditDifficultyMedium.Click += new System.EventHandler(this.radioNoteEditDifficultyMedium_Click);
            // 
            // radioNoteEditDifficultyEasy
            // 
            this.radioNoteEditDifficultyEasy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioNoteEditDifficultyEasy.Location = new System.Drawing.Point(6, 22);
            this.radioNoteEditDifficultyEasy.Name = "radioNoteEditDifficultyEasy";
            this.radioNoteEditDifficultyEasy.Size = new System.Drawing.Size(70, 17);
            this.radioNoteEditDifficultyEasy.TabIndex = 4;
            this.radioNoteEditDifficultyEasy.Text = "Easy";
            this.radioNoteEditDifficultyEasy.UseCompatibleTextRendering = true;
            this.radioNoteEditDifficultyEasy.UseVisualStyleBackColor = true;
            this.radioNoteEditDifficultyEasy.Click += new System.EventHandler(this.radioNoteEditDifficultyEasy_Click);
            // 
            // groupBox37
            // 
            this.groupBox37.BackColor = System.Drawing.Color.Transparent;
            this.groupBox37.Controls.Add(this.checkBoxClearIfNoFrets);
            this.groupBox37.Controls.Add(this.checkBoxPlayMidiStrum);
            this.groupBox37.Controls.Add(this.checkBoxEnableClearTimer);
            this.groupBox37.Controls.Add(this.checkBoxChordStrum);
            this.groupBox37.Controls.Add(this.checkChordMode);
            this.groupBox37.Controls.Add(this.checkThreeNotePowerChord);
            this.groupBox37.Controls.Add(this.checkTwoNotePowerChord);
            this.groupBox37.Controls.Add(this.checkRealtimeNotes);
            this.groupBox37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox37.Location = new System.Drawing.Point(698, 251);
            this.groupBox37.Name = "groupBox37";
            this.groupBox37.Size = new System.Drawing.Size(152, 187);
            this.groupBox37.TabIndex = 8;
            this.groupBox37.TabStop = false;
            this.groupBox37.Text = "Midi Input";
            // 
            // checkBoxClearIfNoFrets
            // 
            this.checkBoxClearIfNoFrets.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxClearIfNoFrets.Location = new System.Drawing.Point(6, 121);
            this.checkBoxClearIfNoFrets.Name = "checkBoxClearIfNoFrets";
            this.checkBoxClearIfNoFrets.Size = new System.Drawing.Size(143, 17);
            this.checkBoxClearIfNoFrets.TabIndex = 6;
            this.checkBoxClearIfNoFrets.Text = "Clear If No Note Held";
            this.checkBoxClearIfNoFrets.UseVisualStyleBackColor = true;
            // 
            // checkBoxPlayMidiStrum
            // 
            this.checkBoxPlayMidiStrum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxPlayMidiStrum.Location = new System.Drawing.Point(6, 103);
            this.checkBoxPlayMidiStrum.Name = "checkBoxPlayMidiStrum";
            this.checkBoxPlayMidiStrum.Size = new System.Drawing.Size(141, 17);
            this.checkBoxPlayMidiStrum.TabIndex = 5;
            this.checkBoxPlayMidiStrum.Text = "Play Midi on Strum";
            this.checkBoxPlayMidiStrum.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableClearTimer
            // 
            this.checkBoxEnableClearTimer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxEnableClearTimer.Location = new System.Drawing.Point(6, 139);
            this.checkBoxEnableClearTimer.Name = "checkBoxEnableClearTimer";
            this.checkBoxEnableClearTimer.Size = new System.Drawing.Size(143, 17);
            this.checkBoxEnableClearTimer.TabIndex = 4;
            this.checkBoxEnableClearTimer.Text = "Enable Clear Timer";
            this.checkBoxEnableClearTimer.UseVisualStyleBackColor = true;
            // 
            // checkBoxChordStrum
            // 
            this.checkBoxChordStrum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxChordStrum.Location = new System.Drawing.Point(31, 86);
            this.checkBoxChordStrum.Name = "checkBoxChordStrum";
            this.checkBoxChordStrum.Size = new System.Drawing.Size(112, 17);
            this.checkBoxChordStrum.TabIndex = 3;
            this.checkBoxChordStrum.Text = "Chord Strum";
            this.checkBoxChordStrum.UseVisualStyleBackColor = true;
            // 
            // checkChordMode
            // 
            this.checkChordMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkChordMode.Location = new System.Drawing.Point(18, 68);
            this.checkChordMode.Name = "checkChordMode";
            this.checkChordMode.Size = new System.Drawing.Size(112, 17);
            this.checkChordMode.TabIndex = 3;
            this.checkChordMode.Text = "Chord Mode";
            this.checkChordMode.UseVisualStyleBackColor = true;
            // 
            // checkThreeNotePowerChord
            // 
            this.checkThreeNotePowerChord.AutoSize = true;
            this.checkThreeNotePowerChord.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkThreeNotePowerChord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkThreeNotePowerChord.Location = new System.Drawing.Point(18, 52);
            this.checkThreeNotePowerChord.Name = "checkThreeNotePowerChord";
            this.checkThreeNotePowerChord.Size = new System.Drawing.Size(107, 16);
            this.checkThreeNotePowerChord.TabIndex = 2;
            this.checkThreeNotePowerChord.Text = "3 Note Power Chord";
            this.checkThreeNotePowerChord.UseVisualStyleBackColor = true;
            // 
            // checkTwoNotePowerChord
            // 
            this.checkTwoNotePowerChord.AutoSize = true;
            this.checkTwoNotePowerChord.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkTwoNotePowerChord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkTwoNotePowerChord.Location = new System.Drawing.Point(18, 35);
            this.checkTwoNotePowerChord.Name = "checkTwoNotePowerChord";
            this.checkTwoNotePowerChord.Size = new System.Drawing.Size(107, 16);
            this.checkTwoNotePowerChord.TabIndex = 1;
            this.checkTwoNotePowerChord.Text = "2 Note Power Chord";
            this.checkTwoNotePowerChord.UseVisualStyleBackColor = true;
            // 
            // checkRealtimeNotes
            // 
            this.checkRealtimeNotes.AutoSize = true;
            this.checkRealtimeNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkRealtimeNotes.Location = new System.Drawing.Point(6, 16);
            this.checkRealtimeNotes.Name = "checkRealtimeNotes";
            this.checkRealtimeNotes.Size = new System.Drawing.Size(98, 17);
            this.checkRealtimeNotes.TabIndex = 0;
            this.checkRealtimeNotes.Text = "Realtime Notes";
            this.checkRealtimeNotes.UseVisualStyleBackColor = true;
            // 
            // groupBox36
            // 
            this.groupBox36.BackColor = System.Drawing.Color.Transparent;
            this.groupBox36.Controls.Add(this.textBox2);
            this.groupBox36.Controls.Add(this.textBox3);
            this.groupBox36.Controls.Add(this.textBox7);
            this.groupBox36.Controls.Add(this.textBox4);
            this.groupBox36.Controls.Add(this.textBox6);
            this.groupBox36.Controls.Add(this.textBox5);
            this.groupBox36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox36.Location = new System.Drawing.Point(3, 6);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Size = new System.Drawing.Size(48, 180);
            this.groupBox36.TabIndex = 1;
            this.groupBox36.TabStop = false;
            this.groupBox36.Text = "Hold";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox2.Location = new System.Drawing.Point(9, 18);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(31, 23);
            this.textBox2.TabIndex = 0;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox3.Location = new System.Drawing.Point(9, 44);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(31, 23);
            this.textBox3.TabIndex = 1;
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox7.Location = new System.Drawing.Point(9, 148);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(31, 23);
            this.textBox7.TabIndex = 5;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox4.Location = new System.Drawing.Point(9, 70);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(31, 23);
            this.textBox4.TabIndex = 2;
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox6.Location = new System.Drawing.Point(9, 122);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(31, 23);
            this.textBox6.TabIndex = 4;
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox5.Location = new System.Drawing.Point(9, 96);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(31, 23);
            this.textBox5.TabIndex = 3;
            // 
            // groupBox34
            // 
            this.groupBox34.BackColor = System.Drawing.Color.Transparent;
            this.groupBox34.Controls.Add(this.checkBoxAllowOverwriteChord);
            this.groupBox34.Controls.Add(this.checkBoxUseCurrentChord);
            this.groupBox34.Controls.Add(this.textBoxPlaceNoteFret);
            this.groupBox34.Controls.Add(this.label21);
            this.groupBox34.Controls.Add(this.label17);
            this.groupBox34.Controls.Add(this.button109);
            this.groupBox34.Controls.Add(this.button108);
            this.groupBox34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox34.Location = new System.Drawing.Point(677, 67);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Size = new System.Drawing.Size(173, 110);
            this.groupBox34.TabIndex = 13;
            this.groupBox34.TabStop = false;
            this.groupBox34.Text = "Click-Create Note";
            // 
            // checkBoxAllowOverwriteChord
            // 
            this.checkBoxAllowOverwriteChord.Checked = true;
            this.checkBoxAllowOverwriteChord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowOverwriteChord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAllowOverwriteChord.Location = new System.Drawing.Point(6, 83);
            this.checkBoxAllowOverwriteChord.Name = "checkBoxAllowOverwriteChord";
            this.checkBoxAllowOverwriteChord.Size = new System.Drawing.Size(97, 17);
            this.checkBoxAllowOverwriteChord.TabIndex = 6;
            this.checkBoxAllowOverwriteChord.Text = "Over-write";
            this.checkBoxAllowOverwriteChord.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseCurrentChord
            // 
            this.checkBoxUseCurrentChord.Checked = true;
            this.checkBoxUseCurrentChord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseCurrentChord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxUseCurrentChord.Location = new System.Drawing.Point(6, 66);
            this.checkBoxUseCurrentChord.Name = "checkBoxUseCurrentChord";
            this.checkBoxUseCurrentChord.Size = new System.Drawing.Size(102, 17);
            this.checkBoxUseCurrentChord.TabIndex = 3;
            this.checkBoxUseCurrentChord.Text = "Use Current";
            this.checkBoxUseCurrentChord.UseVisualStyleBackColor = true;
            this.checkBoxUseCurrentChord.CheckedChanged += new System.EventHandler(this.checkBoxUseCurrentChord_CheckedChanged);
            // 
            // textBoxPlaceNoteFret
            // 
            this.textBoxPlaceNoteFret.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxPlaceNoteFret.Location = new System.Drawing.Point(88, 21);
            this.textBoxPlaceNoteFret.Name = "textBoxPlaceNoteFret";
            this.textBoxPlaceNoteFret.Size = new System.Drawing.Size(27, 23);
            this.textBoxPlaceNoteFret.TabIndex = 5;
            this.textBoxPlaceNoteFret.Text = "0";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(60, 24);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(30, 15);
            this.label21.TabIndex = 4;
            this.label21.Text = "Fret:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(6, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(26, 15);
            this.label17.TabIndex = 0;
            this.label17.Text = "Idle";
            // 
            // button109
            // 
            this.button109.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button109.ImageIndex = 31;
            this.button109.ImageList = this.imageList1;
            this.button109.Location = new System.Drawing.Point(31, 18);
            this.button109.Name = "button109";
            this.button109.Size = new System.Drawing.Size(24, 24);
            this.button109.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button109, "Cancel");
            this.button109.UseVisualStyleBackColor = true;
            this.button109.Click += new System.EventHandler(this.button109_Click);
            // 
            // button108
            // 
            this.button108.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button108.ImageIndex = 34;
            this.button108.ImageList = this.imageList1;
            this.button108.Location = new System.Drawing.Point(6, 18);
            this.button108.Name = "button108";
            this.button108.Size = new System.Drawing.Size(24, 24);
            this.button108.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button108, "Begin");
            this.button108.UseVisualStyleBackColor = true;
            this.button108.Click += new System.EventHandler(this.button108_Click);
            // 
            // groupBox29
            // 
            this.groupBox29.BackColor = System.Drawing.Color.Transparent;
            this.groupBox29.Controls.Add(this.button97);
            this.groupBox29.Controls.Add(this.button98);
            this.groupBox29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox29.Location = new System.Drawing.Point(677, 6);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Size = new System.Drawing.Size(68, 52);
            this.groupBox29.TabIndex = 10;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Midi";
            // 
            // button97
            // 
            this.button97.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button97.ImageIndex = 76;
            this.button97.ImageList = this.imageList1;
            this.button97.Location = new System.Drawing.Point(35, 19);
            this.button97.Name = "button97";
            this.button97.Size = new System.Drawing.Size(24, 24);
            this.button97.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button97, "Stop");
            this.button97.UseVisualStyleBackColor = true;
            this.button97.Click += new System.EventHandler(this.button97_Click);
            // 
            // button98
            // 
            this.button98.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button98.ImageIndex = 77;
            this.button98.ImageList = this.imageList1;
            this.button98.Location = new System.Drawing.Point(6, 19);
            this.button98.Name = "button98";
            this.button98.Size = new System.Drawing.Size(24, 24);
            this.button98.TabIndex = 0;
            this.toolTip1.SetToolTip(this.button98, "Play");
            this.button98.UseVisualStyleBackColor = true;
            this.button98.Click += new System.EventHandler(this.button98_Click);
            // 
            // groupBoxMidiInstrument
            // 
            this.groupBoxMidiInstrument.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxMidiInstrument.Controls.Add(this.labelMidiInputDeviceState);
            this.groupBoxMidiInstrument.Controls.Add(this.button1);
            this.groupBoxMidiInstrument.Controls.Add(this.button2);
            this.groupBoxMidiInstrument.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxMidiInstrument.Location = new System.Drawing.Point(677, 183);
            this.groupBoxMidiInstrument.Name = "groupBoxMidiInstrument";
            this.groupBoxMidiInstrument.Size = new System.Drawing.Size(173, 62);
            this.groupBoxMidiInstrument.TabIndex = 0;
            this.groupBoxMidiInstrument.TabStop = false;
            this.groupBoxMidiInstrument.Text = "MIDI Input Instrument";
            // 
            // labelMidiInputDeviceState
            // 
            this.labelMidiInputDeviceState.AutoSize = true;
            this.labelMidiInputDeviceState.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMidiInputDeviceState.Location = new System.Drawing.Point(6, 42);
            this.labelMidiInputDeviceState.Name = "labelMidiInputDeviceState";
            this.labelMidiInputDeviceState.Size = new System.Drawing.Size(97, 15);
            this.labelMidiInputDeviceState.TabIndex = 3;
            this.labelMidiInputDeviceState.Text = "None Connected";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.ImageIndex = 89;
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(7, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.button1, "Connect midi input device (Squier or Mustang)");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.ImageIndex = 91;
            this.button2.ImageList = this.imageList1;
            this.button2.Location = new System.Drawing.Point(32, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 24);
            this.button2.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button2, "Disconnect Device");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.BackColor = System.Drawing.Color.Transparent;
            this.groupBox13.Controls.Add(this.button33);
            this.groupBox13.Controls.Add(this.button34);
            this.groupBox13.Controls.Add(this.button35);
            this.groupBox13.Controls.Add(this.button36);
            this.groupBox13.Controls.Add(this.button37);
            this.groupBox13.Controls.Add(this.button38);
            this.groupBox13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox13.Location = new System.Drawing.Point(6, 266);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(199, 75);
            this.groupBox13.TabIndex = 9;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Move Note/Chord (Arrow Keys)";
            // 
            // button33
            // 
            this.button33.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button33.Location = new System.Drawing.Point(7, 19);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(63, 23);
            this.button33.TabIndex = 0;
            this.button33.Text = "Up String";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // button34
            // 
            this.button34.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button34.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button34.Location = new System.Drawing.Point(7, 43);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(63, 23);
            this.button34.TabIndex = 1;
            this.button34.Text = "Dn String";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // button35
            // 
            this.button35.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button35.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button35.Location = new System.Drawing.Point(72, 19);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(62, 23);
            this.button35.TabIndex = 2;
            this.button35.Text = "Up Step";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button36
            // 
            this.button36.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button36.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button36.Location = new System.Drawing.Point(72, 43);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(62, 23);
            this.button36.TabIndex = 3;
            this.button36.Text = "Dn Step";
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new System.EventHandler(this.button36_Click);
            // 
            // button37
            // 
            this.button37.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button37.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button37.Location = new System.Drawing.Point(136, 19);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(51, 23);
            this.button37.TabIndex = 4;
            this.button37.Text = "Up Half";
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // button38
            // 
            this.button38.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button38.Location = new System.Drawing.Point(136, 43);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(51, 23);
            this.button38.TabIndex = 5;
            this.button38.Text = "Dn Half";
            this.button38.UseVisualStyleBackColor = true;
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.Color.Transparent;
            this.groupBox12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox12.Controls.Add(this.buttonDeleteSelectedNotes);
            this.groupBox12.Controls.Add(this.buttonPlayHoldBoxMidi);
            this.groupBox12.Controls.Add(this.button4);
            this.groupBox12.Controls.Add(this.button6);
            this.groupBox12.Controls.Add(this.button7);
            this.groupBox12.Controls.Add(this.button55);
            this.groupBox12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox12.Location = new System.Drawing.Point(157, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(48, 180);
            this.groupBox12.TabIndex = 4;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Add";
            // 
            // buttonDeleteSelectedNotes
            // 
            this.buttonDeleteSelectedNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDeleteSelectedNotes.ImageIndex = 31;
            this.buttonDeleteSelectedNotes.ImageList = this.imageList1;
            this.buttonDeleteSelectedNotes.Location = new System.Drawing.Point(11, 146);
            this.buttonDeleteSelectedNotes.Name = "buttonDeleteSelectedNotes";
            this.buttonDeleteSelectedNotes.Size = new System.Drawing.Size(24, 24);
            this.buttonDeleteSelectedNotes.TabIndex = 5;
            this.toolTip1.SetToolTip(this.buttonDeleteSelectedNotes, "Delete Selected Notes (delete button)");
            this.buttonDeleteSelectedNotes.UseVisualStyleBackColor = true;
            this.buttonDeleteSelectedNotes.Click += new System.EventHandler(this.buttonDeleteSelectedNotes_Click);
            // 
            // buttonPlayHoldBoxMidi
            // 
            this.buttonPlayHoldBoxMidi.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonPlayHoldBoxMidi.ImageIndex = 77;
            this.buttonPlayHoldBoxMidi.ImageList = this.imageList1;
            this.buttonPlayHoldBoxMidi.Location = new System.Drawing.Point(11, 120);
            this.buttonPlayHoldBoxMidi.Name = "buttonPlayHoldBoxMidi";
            this.buttonPlayHoldBoxMidi.Size = new System.Drawing.Size(24, 24);
            this.buttonPlayHoldBoxMidi.TabIndex = 4;
            this.toolTip1.SetToolTip(this.buttonPlayHoldBoxMidi, "Play");
            this.buttonPlayHoldBoxMidi.UseVisualStyleBackColor = true;
            this.buttonPlayHoldBoxMidi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonPlayHoldBoxMidi_KeyDown);
            this.buttonPlayHoldBoxMidi.KeyUp += new System.Windows.Forms.KeyEventHandler(this.buttonPlayHoldBoxMidi_KeyUp);
            this.buttonPlayHoldBoxMidi.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button96_MouseDown);
            this.buttonPlayHoldBoxMidi.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button96_MouseUp);
            // 
            // button4
            // 
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.ImageIndex = 35;
            this.button4.ImageList = this.imageList1;
            this.button4.Location = new System.Drawing.Point(11, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 24);
            this.button4.TabIndex = 0;
            this.toolTip1.SetToolTip(this.button4, "Place (V)");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.placeNote_Click);
            // 
            // button6
            // 
            this.button6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button6.ImageIndex = 82;
            this.button6.ImageList = this.imageList1;
            this.button6.Location = new System.Drawing.Point(11, 42);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 24);
            this.button6.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button6, "Clear (C)");
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button7.ImageIndex = 80;
            this.button7.ImageList = this.imageList1;
            this.button7.Location = new System.Drawing.Point(11, 68);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(24, 24);
            this.button7.TabIndex = 2;
            this.button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button7, "Select Next Note");
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.selectNextNote_Click);
            // 
            // button55
            // 
            this.button55.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button55.ImageIndex = 81;
            this.button55.ImageList = this.imageList1;
            this.button55.Location = new System.Drawing.Point(11, 94);
            this.button55.Name = "button55";
            this.button55.Size = new System.Drawing.Size(24, 24);
            this.button55.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button55, "Previous note (shortcut: B button)");
            this.button55.UseVisualStyleBackColor = true;
            this.button55.Click += new System.EventHandler(this.button55_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.Transparent;
            this.groupBox11.Controls.Add(this.buttonReplaceStoredChordWithCopyPattern);
            this.groupBox11.Controls.Add(this.button116);
            this.groupBox11.Controls.Add(this.checkBoxMatchAllFrets);
            this.groupBox11.Controls.Add(this.label15);
            this.groupBox11.Controls.Add(this.label8);
            this.groupBox11.Controls.Add(this.checkBoxSearchByNoteType);
            this.groupBox11.Controls.Add(this.checkBoxSearchByNoteStrum);
            this.groupBox11.Controls.Add(this.button88);
            this.groupBox11.Controls.Add(this.button87);
            this.groupBox11.Controls.Add(this.listBoxStoredChords);
            this.groupBox11.Controls.Add(this.button50);
            this.groupBox11.Controls.Add(this.button51);
            this.groupBox11.Controls.Add(this.button52);
            this.groupBox11.Controls.Add(this.checkBoxSearchByNoteFret);
            this.groupBox11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox11.Location = new System.Drawing.Point(856, 6);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(231, 420);
            this.groupBox11.TabIndex = 14;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Chords (alt+arrow up/down)";
            this.groupBox11.Enter += new System.EventHandler(this.groupBox11_Enter);
            // 
            // buttonReplaceStoredChordWithCopyPattern
            // 
            this.buttonReplaceStoredChordWithCopyPattern.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonReplaceStoredChordWithCopyPattern.ImageIndex = 29;
            this.buttonReplaceStoredChordWithCopyPattern.ImageList = this.imageList1;
            this.buttonReplaceStoredChordWithCopyPattern.Location = new System.Drawing.Point(198, 338);
            this.buttonReplaceStoredChordWithCopyPattern.Name = "buttonReplaceStoredChordWithCopyPattern";
            this.buttonReplaceStoredChordWithCopyPattern.Size = new System.Drawing.Size(24, 24);
            this.buttonReplaceStoredChordWithCopyPattern.TabIndex = 13;
            this.toolTip1.SetToolTip(this.buttonReplaceStoredChordWithCopyPattern, "Replace the matches of the current stored  chord with the current copy pattern");
            this.buttonReplaceStoredChordWithCopyPattern.UseVisualStyleBackColor = true;
            this.buttonReplaceStoredChordWithCopyPattern.Click += new System.EventHandler(this.buttonReplaceStoredChordWithCopyPattern_Click);
            // 
            // button116
            // 
            this.button116.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button116.ImageIndex = 43;
            this.button116.ImageList = this.imageList1;
            this.button116.Location = new System.Drawing.Point(35, 224);
            this.button116.Name = "button116";
            this.button116.Size = new System.Drawing.Size(24, 24);
            this.button116.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button116, "Update");
            this.button116.UseVisualStyleBackColor = true;
            this.button116.Click += new System.EventHandler(this.button116_Click);
            // 
            // checkBoxMatchAllFrets
            // 
            this.checkBoxMatchAllFrets.Checked = true;
            this.checkBoxMatchAllFrets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMatchAllFrets.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchAllFrets.Location = new System.Drawing.Point(30, 277);
            this.checkBoxMatchAllFrets.Name = "checkBoxMatchAllFrets";
            this.checkBoxMatchAllFrets.Size = new System.Drawing.Size(117, 17);
            this.checkBoxMatchAllFrets.TabIndex = 10;
            this.checkBoxMatchAllFrets.Text = "Match All Frets";
            this.checkBoxMatchAllFrets.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(52, 372);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 15);
            this.label15.TabIndex = 6;
            this.label15.Text = "________";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(6, 372);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 15);
            this.label8.TabIndex = 5;
            this.label8.Text = "Result:";
            // 
            // checkBoxSearchByNoteType
            // 
            this.checkBoxSearchByNoteType.Checked = true;
            this.checkBoxSearchByNoteType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchByNoteType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSearchByNoteType.Location = new System.Drawing.Point(9, 296);
            this.checkBoxSearchByNoteType.Name = "checkBoxSearchByNoteType";
            this.checkBoxSearchByNoteType.Size = new System.Drawing.Size(145, 17);
            this.checkBoxSearchByNoteType.TabIndex = 11;
            this.checkBoxSearchByNoteType.Text = "Match on Note Type";
            this.checkBoxSearchByNoteType.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchByNoteStrum
            // 
            this.checkBoxSearchByNoteStrum.Checked = true;
            this.checkBoxSearchByNoteStrum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchByNoteStrum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSearchByNoteStrum.Location = new System.Drawing.Point(9, 315);
            this.checkBoxSearchByNoteStrum.Name = "checkBoxSearchByNoteStrum";
            this.checkBoxSearchByNoteStrum.Size = new System.Drawing.Size(152, 17);
            this.checkBoxSearchByNoteStrum.TabIndex = 12;
            this.checkBoxSearchByNoteStrum.Text = "Match on Strum Mode";
            this.checkBoxSearchByNoteStrum.UseVisualStyleBackColor = true;
            // 
            // button88
            // 
            this.button88.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button88.ImageIndex = 81;
            this.button88.ImageList = this.imageList1;
            this.button88.Location = new System.Drawing.Point(9, 338);
            this.button88.Name = "button88";
            this.button88.Size = new System.Drawing.Size(24, 24);
            this.button88.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button88, "Find Prev");
            this.button88.UseVisualStyleBackColor = true;
            this.button88.Click += new System.EventHandler(this.button88_Click);
            // 
            // button87
            // 
            this.button87.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button87.ImageIndex = 80;
            this.button87.ImageList = this.imageList1;
            this.button87.Location = new System.Drawing.Point(38, 338);
            this.button87.Name = "button87";
            this.button87.Size = new System.Drawing.Size(24, 24);
            this.button87.TabIndex = 8;
            this.toolTip1.SetToolTip(this.button87, "Find Next");
            this.button87.UseVisualStyleBackColor = true;
            this.button87.Click += new System.EventHandler(this.button87_Click);
            // 
            // listBoxStoredChords
            // 
            this.listBoxStoredChords.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxStoredChords.FormattingEnabled = true;
            this.listBoxStoredChords.ItemHeight = 15;
            this.listBoxStoredChords.Location = new System.Drawing.Point(6, 19);
            this.listBoxStoredChords.Name = "listBoxStoredChords";
            this.listBoxStoredChords.Size = new System.Drawing.Size(216, 199);
            this.listBoxStoredChords.TabIndex = 0;
            this.listBoxStoredChords.Click += new System.EventHandler(this.listBox6_Click);
            this.listBoxStoredChords.SelectedIndexChanged += new System.EventHandler(this.listBox6_SelectedIndexChanged);
            // 
            // button50
            // 
            this.button50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button50.ImageIndex = 14;
            this.button50.ImageList = this.imageList1;
            this.button50.Location = new System.Drawing.Point(6, 224);
            this.button50.Name = "button50";
            this.button50.Size = new System.Drawing.Size(24, 24);
            this.button50.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button50, "Store Chord (G)");
            this.button50.UseVisualStyleBackColor = true;
            this.button50.Click += new System.EventHandler(this.button50_Click);
            // 
            // button51
            // 
            this.button51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button51.ImageIndex = 31;
            this.button51.ImageList = this.imageList1;
            this.button51.Location = new System.Drawing.Point(198, 224);
            this.button51.Name = "button51";
            this.button51.Size = new System.Drawing.Size(24, 24);
            this.button51.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button51, "Delete");
            this.button51.UseVisualStyleBackColor = true;
            this.button51.Click += new System.EventHandler(this.button51_Click);
            // 
            // button52
            // 
            this.button52.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button52.ImageIndex = 1;
            this.button52.ImageList = this.imageList1;
            this.button52.Location = new System.Drawing.Point(198, 253);
            this.button52.Name = "button52";
            this.button52.Size = new System.Drawing.Size(24, 24);
            this.button52.TabIndex = 4;
            this.toolTip1.SetToolTip(this.button52, "Clear");
            this.button52.UseVisualStyleBackColor = true;
            this.button52.Click += new System.EventHandler(this.button52_Click);
            // 
            // checkBoxSearchByNoteFret
            // 
            this.checkBoxSearchByNoteFret.Checked = true;
            this.checkBoxSearchByNoteFret.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchByNoteFret.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSearchByNoteFret.Location = new System.Drawing.Point(9, 258);
            this.checkBoxSearchByNoteFret.Name = "checkBoxSearchByNoteFret";
            this.checkBoxSearchByNoteFret.Size = new System.Drawing.Size(148, 17);
            this.checkBoxSearchByNoteFret.TabIndex = 9;
            this.checkBoxSearchByNoteFret.Text = "Match on Same Frets";
            this.checkBoxSearchByNoteFret.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.Transparent;
            this.groupBox10.Controls.Add(this.buttonSelectForward);
            this.groupBox10.Controls.Add(this.buttonFixOverlappingNotes);
            this.groupBox10.Controls.Add(this.buttonNoteUtilSelectAll);
            this.groupBox10.Controls.Add(this.buttonUtilMethodSetToG5);
            this.groupBox10.Controls.Add(this.buttonUtilMethodFindNoteLenZero);
            this.groupBox10.Controls.Add(this.buttonDownOctave);
            this.groupBox10.Controls.Add(this.buttonUpOctave);
            this.groupBox10.Controls.Add(this.buttonUtilMethodSnapToG5);
            this.groupBox10.Controls.Add(this.button132);
            this.groupBox10.Controls.Add(this.buttonUp12);
            this.groupBox10.Controls.Add(this.buttonDownString);
            this.groupBox10.Controls.Add(this.button76);
            this.groupBox10.Controls.Add(this.button124);
            this.groupBox10.Controls.Add(this.button122);
            this.groupBox10.Controls.Add(this.buttonAddHammeron);
            this.groupBox10.Controls.Add(this.button85);
            this.groupBox10.Controls.Add(this.buttonAddSlideHammeron);
            this.groupBox10.Controls.Add(this.button32);
            this.groupBox10.Controls.Add(this.button53);
            this.groupBox10.Controls.Add(this.buttonSelectBack);
            this.groupBox10.Controls.Add(this.button54);
            this.groupBox10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox10.Location = new System.Drawing.Point(211, 273);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(481, 221);
            this.groupBox10.TabIndex = 11;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Utility Methods";
            this.groupBox10.Enter += new System.EventHandler(this.groupBox10_Enter);
            // 
            // buttonSelectForward
            // 
            this.buttonSelectForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonSelectForward.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSelectForward.Location = new System.Drawing.Point(5, 108);
            this.buttonSelectForward.Name = "buttonSelectForward";
            this.buttonSelectForward.Size = new System.Drawing.Size(103, 23);
            this.buttonSelectForward.TabIndex = 21;
            this.buttonSelectForward.Text = "Select Forward";
            this.toolTip1.SetToolTip(this.buttonSelectForward, "Snaps all chords");
            this.buttonSelectForward.UseVisualStyleBackColor = true;
            this.buttonSelectForward.Click += new System.EventHandler(this.buttonSelectForward_Click);
            // 
            // buttonFixOverlappingNotes
            // 
            this.buttonFixOverlappingNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonFixOverlappingNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonFixOverlappingNotes.Location = new System.Drawing.Point(319, 108);
            this.buttonFixOverlappingNotes.Name = "buttonFixOverlappingNotes";
            this.buttonFixOverlappingNotes.Size = new System.Drawing.Size(143, 23);
            this.buttonFixOverlappingNotes.TabIndex = 20;
            this.buttonFixOverlappingNotes.Text = "Fix Overlapping Notes";
            this.toolTip1.SetToolTip(this.buttonFixOverlappingNotes, "Snaps all chords");
            this.buttonFixOverlappingNotes.UseVisualStyleBackColor = true;
            this.buttonFixOverlappingNotes.Click += new System.EventHandler(this.buttonFixOverlappingNotes_Click);
            // 
            // buttonNoteUtilSelectAll
            // 
            this.buttonNoteUtilSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonNoteUtilSelectAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonNoteUtilSelectAll.Location = new System.Drawing.Point(94, 39);
            this.buttonNoteUtilSelectAll.Name = "buttonNoteUtilSelectAll";
            this.buttonNoteUtilSelectAll.Size = new System.Drawing.Size(67, 23);
            this.buttonNoteUtilSelectAll.TabIndex = 19;
            this.buttonNoteUtilSelectAll.Text = "Select All";
            this.toolTip1.SetToolTip(this.buttonNoteUtilSelectAll, "Select All Chords");
            this.buttonNoteUtilSelectAll.UseVisualStyleBackColor = true;
            this.buttonNoteUtilSelectAll.Click += new System.EventHandler(this.buttonNoteUtilSelectAll_Click);
            // 
            // buttonUtilMethodSetToG5
            // 
            this.buttonUtilMethodSetToG5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonUtilMethodSetToG5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUtilMethodSetToG5.Location = new System.Drawing.Point(408, 62);
            this.buttonUtilMethodSetToG5.Name = "buttonUtilMethodSetToG5";
            this.buttonUtilMethodSetToG5.Size = new System.Drawing.Size(54, 23);
            this.buttonUtilMethodSetToG5.TabIndex = 18;
            this.buttonUtilMethodSetToG5.Text = "Set";
            this.toolTip1.SetToolTip(this.buttonUtilMethodSetToG5, "Set length to same as Guitar5 Length");
            this.buttonUtilMethodSetToG5.UseVisualStyleBackColor = true;
            this.buttonUtilMethodSetToG5.Click += new System.EventHandler(this.buttonUtilMethodSetToG5_Click);
            // 
            // buttonUtilMethodFindNoteLenZero
            // 
            this.buttonUtilMethodFindNoteLenZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonUtilMethodFindNoteLenZero.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUtilMethodFindNoteLenZero.Location = new System.Drawing.Point(319, 16);
            this.buttonUtilMethodFindNoteLenZero.Name = "buttonUtilMethodFindNoteLenZero";
            this.buttonUtilMethodFindNoteLenZero.Size = new System.Drawing.Size(143, 23);
            this.buttonUtilMethodFindNoteLenZero.TabIndex = 17;
            this.buttonUtilMethodFindNoteLenZero.Text = "Find Note Zero Length";
            this.toolTip1.SetToolTip(this.buttonUtilMethodFindNoteLenZero, "Find note with zero length");
            this.buttonUtilMethodFindNoteLenZero.UseVisualStyleBackColor = true;
            this.buttonUtilMethodFindNoteLenZero.Click += new System.EventHandler(this.buttonUtilMethodFindNoteLenZero_Click);
            // 
            // buttonDownOctave
            // 
            this.buttonDownOctave.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonDownOctave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDownOctave.Location = new System.Drawing.Point(252, 62);
            this.buttonDownOctave.Name = "buttonDownOctave";
            this.buttonDownOctave.Size = new System.Drawing.Size(66, 23);
            this.buttonDownOctave.TabIndex = 16;
            this.buttonDownOctave.Text = "Down";
            this.toolTip1.SetToolTip(this.buttonDownOctave, "Move Selected Notes Down One Octave");
            this.buttonDownOctave.UseVisualStyleBackColor = true;
            this.buttonDownOctave.Click += new System.EventHandler(this.buttonDownOctave_Click);
            // 
            // buttonUpOctave
            // 
            this.buttonUpOctave.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonUpOctave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUpOctave.Location = new System.Drawing.Point(162, 62);
            this.buttonUpOctave.Name = "buttonUpOctave";
            this.buttonUpOctave.Size = new System.Drawing.Size(88, 23);
            this.buttonUpOctave.TabIndex = 15;
            this.buttonUpOctave.Text = "Up Octave";
            this.toolTip1.SetToolTip(this.buttonUpOctave, "Move Selected Notes Up One Octave");
            this.buttonUpOctave.UseVisualStyleBackColor = true;
            this.buttonUpOctave.Click += new System.EventHandler(this.buttonUpOctave_Click);
            // 
            // buttonUtilMethodSnapToG5
            // 
            this.buttonUtilMethodSnapToG5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonUtilMethodSnapToG5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUtilMethodSnapToG5.Location = new System.Drawing.Point(319, 62);
            this.buttonUtilMethodSnapToG5.Name = "buttonUtilMethodSnapToG5";
            this.buttonUtilMethodSnapToG5.Size = new System.Drawing.Size(88, 23);
            this.buttonUtilMethodSnapToG5.TabIndex = 14;
            this.buttonUtilMethodSnapToG5.Text = "Snap To G5";
            this.toolTip1.SetToolTip(this.buttonUtilMethodSnapToG5, "Snap notes to guitar5 track");
            this.buttonUtilMethodSnapToG5.UseVisualStyleBackColor = true;
            this.buttonUtilMethodSnapToG5.Click += new System.EventHandler(this.button133_Click);
            // 
            // button132
            // 
            this.button132.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button132.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button132.Location = new System.Drawing.Point(319, 39);
            this.button132.Name = "button132";
            this.button132.Size = new System.Drawing.Size(143, 23);
            this.button132.TabIndex = 13;
            this.button132.Text = "Find Note over 17";
            this.toolTip1.SetToolTip(this.button132, "Find Note with fret over 17");
            this.button132.UseVisualStyleBackColor = true;
            this.button132.Click += new System.EventHandler(this.button132_Click);
            // 
            // buttonUp12
            // 
            this.buttonUp12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonUp12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUp12.Location = new System.Drawing.Point(162, 16);
            this.buttonUp12.Name = "buttonUp12";
            this.buttonUp12.Size = new System.Drawing.Size(88, 23);
            this.buttonUp12.TabIndex = 12;
            this.buttonUp12.Text = "Up 12";
            this.toolTip1.SetToolTip(this.buttonUp12, "Move Selected Notes Up 12 Frets");
            this.buttonUp12.UseVisualStyleBackColor = true;
            this.buttonUp12.Click += new System.EventHandler(this.buttonUp12_Click);
            // 
            // buttonDownString
            // 
            this.buttonDownString.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonDownString.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDownString.Location = new System.Drawing.Point(252, 39);
            this.buttonDownString.Name = "buttonDownString";
            this.buttonDownString.Size = new System.Drawing.Size(66, 23);
            this.buttonDownString.TabIndex = 11;
            this.buttonDownString.Text = "Down";
            this.toolTip1.SetToolTip(this.buttonDownString, "Move Selected Notes Down One String and Up Five Frets");
            this.buttonDownString.UseVisualStyleBackColor = true;
            this.buttonDownString.Click += new System.EventHandler(this.buttonDownString_Click);
            // 
            // button76
            // 
            this.button76.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button76.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button76.Location = new System.Drawing.Point(319, 85);
            this.button76.Name = "button76";
            this.button76.Size = new System.Drawing.Size(143, 23);
            this.button76.TabIndex = 10;
            this.button76.Text = "Clear All Strum";
            this.toolTip1.SetToolTip(this.button76, "Delete all High,  Medium and Low Strum Markers");
            this.button76.UseVisualStyleBackColor = true;
            this.button76.Click += new System.EventHandler(this.buttonClearAllStrum_Click);
            // 
            // button124
            // 
            this.button124.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button124.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button124.Location = new System.Drawing.Point(162, 39);
            this.button124.Name = "button124";
            this.button124.Size = new System.Drawing.Size(88, 23);
            this.button124.TabIndex = 9;
            this.button124.Text = "Up String";
            this.toolTip1.SetToolTip(this.button124, "Move Selected Notes Up One String and Down Five Frets");
            this.button124.UseVisualStyleBackColor = true;
            this.button124.Click += new System.EventHandler(this.button124_Click);
            // 
            // button122
            // 
            this.button122.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button122.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button122.Location = new System.Drawing.Point(252, 16);
            this.button122.Name = "button122";
            this.button122.Size = new System.Drawing.Size(66, 23);
            this.button122.TabIndex = 8;
            this.button122.Text = "Down";
            this.toolTip1.SetToolTip(this.button122, "Move Selected Notes Down 12 Frets");
            this.button122.UseVisualStyleBackColor = true;
            this.button122.Click += new System.EventHandler(this.button122_Click);
            // 
            // buttonAddHammeron
            // 
            this.buttonAddHammeron.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonAddHammeron.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonAddHammeron.Location = new System.Drawing.Point(5, 62);
            this.buttonAddHammeron.Name = "buttonAddHammeron";
            this.buttonAddHammeron.Size = new System.Drawing.Size(88, 23);
            this.buttonAddHammeron.TabIndex = 2;
            this.buttonAddHammeron.Text = "HO/PO (Q)";
            this.toolTip1.SetToolTip(this.buttonAddHammeron, "Force Hammeron or Pulloff for selected notes");
            this.buttonAddHammeron.UseVisualStyleBackColor = true;
            this.buttonAddHammeron.Click += new System.EventHandler(this.button86_Click);
            // 
            // button85
            // 
            this.button85.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button85.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button85.Location = new System.Drawing.Point(5, 39);
            this.button85.Name = "button85";
            this.button85.Size = new System.Drawing.Size(88, 23);
            this.button85.TabIndex = 1;
            this.button85.Text = "Slide (W)";
            this.toolTip1.SetToolTip(this.button85, "Create slide modifier for selected notes");
            this.button85.UseVisualStyleBackColor = true;
            this.button85.Click += new System.EventHandler(this.button85_Click);
            // 
            // buttonAddSlideHammeron
            // 
            this.buttonAddSlideHammeron.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonAddSlideHammeron.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonAddSlideHammeron.Location = new System.Drawing.Point(5, 16);
            this.buttonAddSlideHammeron.Name = "buttonAddSlideHammeron";
            this.buttonAddSlideHammeron.Size = new System.Drawing.Size(88, 23);
            this.buttonAddSlideHammeron.TabIndex = 0;
            this.buttonAddSlideHammeron.Text = "SL+HOPO (E)";
            this.toolTip1.SetToolTip(this.buttonAddSlideHammeron, "Add a slide to the selected note and a hammer on to the next note");
            this.buttonAddSlideHammeron.UseVisualStyleBackColor = true;
            this.buttonAddSlideHammeron.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button32
            // 
            this.button32.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button32.Location = new System.Drawing.Point(94, 85);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(67, 23);
            this.button32.TabIndex = 3;
            this.button32.Text = "Combine";
            this.toolTip1.SetToolTip(this.button32, "Combine the currently selected note with the next note, extending the end of the " +
        "selected note to the end of the next note.");
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button53
            // 
            this.button53.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button53.Location = new System.Drawing.Point(5, 85);
            this.button53.Name = "button53";
            this.button53.Size = new System.Drawing.Size(88, 23);
            this.button53.TabIndex = 4;
            this.button53.Text = "Extend";
            this.toolTip1.SetToolTip(this.button53, "Extend the end of the selected note to the beginning of the next note");
            this.button53.UseVisualStyleBackColor = true;
            this.button53.Click += new System.EventHandler(this.button53_Click);
            // 
            // buttonSelectBack
            // 
            this.buttonSelectBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.buttonSelectBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSelectBack.Location = new System.Drawing.Point(109, 108);
            this.buttonSelectBack.Name = "buttonSelectBack";
            this.buttonSelectBack.Size = new System.Drawing.Size(52, 23);
            this.buttonSelectBack.TabIndex = 5;
            this.buttonSelectBack.Text = "Back";
            this.buttonSelectBack.UseVisualStyleBackColor = true;
            this.buttonSelectBack.Click += new System.EventHandler(this.buttonSelectBack_Click);
            // 
            // button54
            // 
            this.button54.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.button54.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button54.Location = new System.Drawing.Point(94, 16);
            this.button54.Name = "button54";
            this.button54.Size = new System.Drawing.Size(67, 23);
            this.button54.TabIndex = 5;
            this.button54.Text = "Split";
            this.button54.UseVisualStyleBackColor = true;
            this.button54.Click += new System.EventHandler(this.button54_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.Transparent;
            this.groupBox9.Controls.Add(this.textBoxNoteEditorSelectedChordDownTick);
            this.groupBox9.Controls.Add(this.textBoxNoteEditorSelectedChordUpTick);
            this.groupBox9.Controls.Add(this.textBoxNoteEditorSelectedChordTickLength);
            this.groupBox9.Controls.Add(this.button11);
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label40);
            this.groupBox9.Controls.Add(this.label24);
            this.groupBox9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox9.Location = new System.Drawing.Point(4, 189);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(201, 71);
            this.groupBox9.TabIndex = 10;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Note Location";
            // 
            // textBoxNoteEditorSelectedChordDownTick
            // 
            this.textBoxNoteEditorSelectedChordDownTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordDownTick.Location = new System.Drawing.Point(7, 38);
            this.textBoxNoteEditorSelectedChordDownTick.Name = "textBoxNoteEditorSelectedChordDownTick";
            this.textBoxNoteEditorSelectedChordDownTick.Size = new System.Drawing.Size(53, 23);
            this.textBoxNoteEditorSelectedChordDownTick.TabIndex = 0;
            // 
            // textBoxNoteEditorSelectedChordUpTick
            // 
            this.textBoxNoteEditorSelectedChordUpTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordUpTick.Location = new System.Drawing.Point(68, 38);
            this.textBoxNoteEditorSelectedChordUpTick.Name = "textBoxNoteEditorSelectedChordUpTick";
            this.textBoxNoteEditorSelectedChordUpTick.Size = new System.Drawing.Size(53, 23);
            this.textBoxNoteEditorSelectedChordUpTick.TabIndex = 1;
            this.textBoxNoteEditorSelectedChordUpTick.TextChanged += new System.EventHandler(this.textBox20_TextChanged);
            // 
            // textBoxNoteEditorSelectedChordTickLength
            // 
            this.textBoxNoteEditorSelectedChordTickLength.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordTickLength.Location = new System.Drawing.Point(126, 38);
            this.textBoxNoteEditorSelectedChordTickLength.Name = "textBoxNoteEditorSelectedChordTickLength";
            this.textBoxNoteEditorSelectedChordTickLength.Size = new System.Drawing.Size(41, 23);
            this.textBoxNoteEditorSelectedChordTickLength.TabIndex = 2;
            this.textBoxNoteEditorSelectedChordTickLength.TextChanged += new System.EventHandler(this.textBox19_TextChanged);
            // 
            // button11
            // 
            this.button11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button11.ImageIndex = 13;
            this.button11.ImageList = this.imageList1;
            this.button11.Location = new System.Drawing.Point(171, 36);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(24, 24);
            this.button11.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button11, "Update");
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(5, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 15);
            this.label9.TabIndex = 45;
            this.label9.Text = "Start:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label40.Location = new System.Drawing.Point(66, 19);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(30, 15);
            this.label40.TabIndex = 45;
            this.label40.Text = "End:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label24.Location = new System.Drawing.Point(123, 19);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(47, 15);
            this.label24.TabIndex = 67;
            this.label24.Text = "Length:";
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.Transparent;
            this.groupBox8.Controls.Add(this.checkIsHammeron);
            this.groupBox8.Controls.Add(this.checkIsTap);
            this.groupBox8.Controls.Add(this.checkIsX);
            this.groupBox8.Controls.Add(this.checkIsSlideReversed);
            this.groupBox8.Controls.Add(this.checkIsSlide);
            this.groupBox8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox8.Location = new System.Drawing.Point(233, 9);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(106, 137);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Note Type";
            // 
            // checkIsHammeron
            // 
            this.checkIsHammeron.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkIsHammeron.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIsHammeron.Location = new System.Drawing.Point(6, 54);
            this.checkIsHammeron.Name = "checkIsHammeron";
            this.checkIsHammeron.Size = new System.Drawing.Size(92, 17);
            this.checkIsHammeron.TabIndex = 2;
            this.checkIsHammeron.Text = "HO/PO (H)";
            this.toolTip1.SetToolTip(this.checkIsHammeron, "Hammer-on or Pull-off note");
            this.checkIsHammeron.UseVisualStyleBackColor = true;
            this.checkIsHammeron.CheckedChanged += new System.EventHandler(this.checkIsHammeron_CheckedChanged);
            // 
            // checkIsTap
            // 
            this.checkIsTap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIsTap.Location = new System.Drawing.Point(6, 72);
            this.checkIsTap.Name = "checkIsTap";
            this.checkIsTap.Size = new System.Drawing.Size(92, 18);
            this.checkIsTap.TabIndex = 3;
            this.checkIsTap.Text = "Is Tap (T)";
            this.checkIsTap.UseVisualStyleBackColor = true;
            this.checkIsTap.CheckedChanged += new System.EventHandler(this.checkIsTap_CheckedChanged);
            // 
            // checkIsX
            // 
            this.checkIsX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIsX.Location = new System.Drawing.Point(6, 91);
            this.checkIsX.Name = "checkIsX";
            this.checkIsX.Size = new System.Drawing.Size(96, 19);
            this.checkIsX.TabIndex = 4;
            this.checkIsX.Text = "Is X Note (X)";
            this.checkIsX.UseVisualStyleBackColor = true;
            this.checkIsX.CheckedChanged += new System.EventHandler(this.checkIsX_CheckedChanged);
            // 
            // checkIsSlideReversed
            // 
            this.checkIsSlideReversed.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkIsSlideReversed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIsSlideReversed.Location = new System.Drawing.Point(19, 36);
            this.checkIsSlideReversed.Name = "checkIsSlideReversed";
            this.checkIsSlideReversed.Size = new System.Drawing.Size(79, 16);
            this.checkIsSlideReversed.TabIndex = 1;
            this.checkIsSlideReversed.Text = "Reverse (R)";
            this.checkIsSlideReversed.UseVisualStyleBackColor = true;
            // 
            // checkIsSlide
            // 
            this.checkIsSlide.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIsSlide.Location = new System.Drawing.Point(6, 17);
            this.checkIsSlide.Name = "checkIsSlide";
            this.checkIsSlide.Size = new System.Drawing.Size(92, 18);
            this.checkIsSlide.TabIndex = 0;
            this.checkIsSlide.Text = "Is Slide (S)";
            this.checkIsSlide.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.checkBoxAutoSelectNext);
            this.groupBox7.Controls.Add(this.checkBoxClearAfterNote);
            this.groupBox7.Controls.Add(this.checkIndentBString);
            this.groupBox7.Controls.Add(this.checkScrollToSelection);
            this.groupBox7.Controls.Add(this.checkKBQuickEdit);
            this.groupBox7.Controls.Add(this.checkKeepSelection);
            this.groupBox7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox7.Location = new System.Drawing.Point(345, 9);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(156, 137);
            this.groupBox7.TabIndex = 7;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Input Modifiers";
            // 
            // checkBoxAutoSelectNext
            // 
            this.checkBoxAutoSelectNext.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxAutoSelectNext.Location = new System.Drawing.Point(6, 110);
            this.checkBoxAutoSelectNext.Name = "checkBoxAutoSelectNext";
            this.checkBoxAutoSelectNext.Size = new System.Drawing.Size(134, 17);
            this.checkBoxAutoSelectNext.TabIndex = 5;
            this.checkBoxAutoSelectNext.Text = "Auto Select Next";
            this.toolTip1.SetToolTip(this.checkBoxAutoSelectNext, "Selects the next note automatically after placing a note");
            this.checkBoxAutoSelectNext.UseVisualStyleBackColor = true;
            // 
            // checkBoxClearAfterNote
            // 
            this.checkBoxClearAfterNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxClearAfterNote.Location = new System.Drawing.Point(6, 37);
            this.checkBoxClearAfterNote.Name = "checkBoxClearAfterNote";
            this.checkBoxClearAfterNote.Size = new System.Drawing.Size(135, 18);
            this.checkBoxClearAfterNote.TabIndex = 1;
            this.checkBoxClearAfterNote.Text = "Clear After Note (J)";
            this.checkBoxClearAfterNote.UseVisualStyleBackColor = true;
            // 
            // checkIndentBString
            // 
            this.checkIndentBString.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkIndentBString.Location = new System.Drawing.Point(6, 91);
            this.checkIndentBString.Name = "checkIndentBString";
            this.checkIndentBString.Size = new System.Drawing.Size(116, 19);
            this.checkIndentBString.TabIndex = 4;
            this.checkIndentBString.Text = "Indent B String";
            this.checkIndentBString.UseVisualStyleBackColor = true;
            // 
            // checkScrollToSelection
            // 
            this.checkScrollToSelection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkScrollToSelection.Location = new System.Drawing.Point(6, 55);
            this.checkScrollToSelection.Name = "checkScrollToSelection";
            this.checkScrollToSelection.Size = new System.Drawing.Size(135, 17);
            this.checkScrollToSelection.TabIndex = 2;
            this.checkScrollToSelection.Text = "Scroll To Selection";
            this.checkScrollToSelection.UseVisualStyleBackColor = true;
            // 
            // checkKBQuickEdit
            // 
            this.checkKBQuickEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkKBQuickEdit.Location = new System.Drawing.Point(6, 73);
            this.checkKBQuickEdit.Name = "checkKBQuickEdit";
            this.checkKBQuickEdit.Size = new System.Drawing.Size(134, 17);
            this.checkKBQuickEdit.TabIndex = 3;
            this.checkKBQuickEdit.Text = "Keyboard Quick Edit";
            this.checkKBQuickEdit.UseVisualStyleBackColor = true;
            // 
            // checkKeepSelection
            // 
            this.checkKeepSelection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkKeepSelection.Location = new System.Drawing.Point(6, 19);
            this.checkKeepSelection.Name = "checkKeepSelection";
            this.checkKeepSelection.Size = new System.Drawing.Size(134, 18);
            this.checkKeepSelection.TabIndex = 0;
            this.checkKeepSelection.Text = "Keep Selection (K)";
            this.checkKeepSelection.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox0);
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox5);
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox4);
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox3);
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox2);
            this.groupBox6.Controls.Add(this.textBoxNoteEditorSelectedChordChannelBox1);
            this.groupBox6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox6.Location = new System.Drawing.Point(103, 6);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox6.Size = new System.Drawing.Size(49, 180);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Chan";
            this.toolTip1.SetToolTip(this.groupBox6, "Midi Channel - Advanced");
            // 
            // textBoxNoteEditorSelectedChordChannelBox0
            // 
            this.textBoxNoteEditorSelectedChordChannelBox0.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox0.Location = new System.Drawing.Point(8, 18);
            this.textBoxNoteEditorSelectedChordChannelBox0.Name = "textBoxNoteEditorSelectedChordChannelBox0";
            this.textBoxNoteEditorSelectedChordChannelBox0.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox0.TabIndex = 0;
            // 
            // contextMenuStripChannels
            // 
            this.contextMenuStripChannels.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem4,
            this.toolStripMenuItem8,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.contextMenuStripChannels.Name = "contextMenuStrip1";
            this.contextMenuStripChannels.Size = new System.Drawing.Size(136, 114);
            this.contextMenuStripChannels.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripChannels_Opening);
            this.contextMenuStripChannels.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripChannels_ItemClicked);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(135, 22);
            this.toolStripMenuItem5.Text = "0 - Normal";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(135, 22);
            this.toolStripMenuItem4.Text = "3 - X Note";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(135, 22);
            this.toolStripMenuItem8.Text = "4 - Tap";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(135, 22);
            this.toolStripMenuItem6.Text = "1 - Helper";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(135, 22);
            this.toolStripMenuItem7.Text = "String Bend";
            // 
            // textBoxNoteEditorSelectedChordChannelBox5
            // 
            this.textBoxNoteEditorSelectedChordChannelBox5.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox5.Location = new System.Drawing.Point(8, 148);
            this.textBoxNoteEditorSelectedChordChannelBox5.Name = "textBoxNoteEditorSelectedChordChannelBox5";
            this.textBoxNoteEditorSelectedChordChannelBox5.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox5.TabIndex = 5;
            // 
            // textBoxNoteEditorSelectedChordChannelBox4
            // 
            this.textBoxNoteEditorSelectedChordChannelBox4.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox4.Location = new System.Drawing.Point(8, 122);
            this.textBoxNoteEditorSelectedChordChannelBox4.Name = "textBoxNoteEditorSelectedChordChannelBox4";
            this.textBoxNoteEditorSelectedChordChannelBox4.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox4.TabIndex = 4;
            // 
            // textBoxNoteEditorSelectedChordChannelBox3
            // 
            this.textBoxNoteEditorSelectedChordChannelBox3.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox3.Location = new System.Drawing.Point(8, 96);
            this.textBoxNoteEditorSelectedChordChannelBox3.Name = "textBoxNoteEditorSelectedChordChannelBox3";
            this.textBoxNoteEditorSelectedChordChannelBox3.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox3.TabIndex = 3;
            // 
            // textBoxNoteEditorSelectedChordChannelBox2
            // 
            this.textBoxNoteEditorSelectedChordChannelBox2.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox2.Location = new System.Drawing.Point(8, 70);
            this.textBoxNoteEditorSelectedChordChannelBox2.Name = "textBoxNoteEditorSelectedChordChannelBox2";
            this.textBoxNoteEditorSelectedChordChannelBox2.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox2.TabIndex = 2;
            // 
            // textBoxNoteEditorSelectedChordChannelBox1
            // 
            this.textBoxNoteEditorSelectedChordChannelBox1.ContextMenuStrip = this.contextMenuStripChannels;
            this.textBoxNoteEditorSelectedChordChannelBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteEditorSelectedChordChannelBox1.Location = new System.Drawing.Point(8, 44);
            this.textBoxNoteEditorSelectedChordChannelBox1.Name = "textBoxNoteEditorSelectedChordChannelBox1";
            this.textBoxNoteEditorSelectedChordChannelBox1.Size = new System.Drawing.Size(31, 23);
            this.textBoxNoteEditorSelectedChordChannelBox1.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.checkBoxMatchRemoveExisting);
            this.groupBox5.Controls.Add(this.buttonNoteEditorCopyPatternPresetUpdate);
            this.groupBox5.Controls.Add(this.buttonNoteEditorCopyPatternPresetRemove);
            this.groupBox5.Controls.Add(this.buttonNoteEditorCopyPatternPresetCreate);
            this.groupBox5.Controls.Add(this.comboNoteEditorCopyPatternPreset);
            this.groupBox5.Controls.Add(this.label48);
            this.groupBox5.Controls.Add(this.button120);
            this.groupBox5.Controls.Add(this.button121);
            this.groupBox5.Controls.Add(this.checkBoxMatchForwardOnly);
            this.groupBox5.Controls.Add(this.checkMatchBeat);
            this.groupBox5.Controls.Add(this.checkBoxKeepLengths);
            this.groupBox5.Controls.Add(this.checkBoxMatchSpacing);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.checkBoxFirstMatchOnly);
            this.groupBox5.Controls.Add(this.button29);
            this.groupBox5.Controls.Add(this.checkBoxMatchLength6);
            this.groupBox5.Controls.Add(this.checkBoxMatchLengths);
            this.groupBox5.Controls.Add(this.button28);
            this.groupBox5.Controls.Add(this.button30);
            this.groupBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox5.Location = new System.Drawing.Point(521, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(150, 251);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Copy Pattern (P)";
            // 
            // checkBoxMatchRemoveExisting
            // 
            this.checkBoxMatchRemoveExisting.AutoSize = true;
            this.checkBoxMatchRemoveExisting.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchRemoveExisting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchRemoveExisting.Location = new System.Drawing.Point(9, 152);
            this.checkBoxMatchRemoveExisting.Name = "checkBoxMatchRemoveExisting";
            this.checkBoxMatchRemoveExisting.Size = new System.Drawing.Size(95, 16);
            this.checkBoxMatchRemoveExisting.TabIndex = 18;
            this.checkBoxMatchRemoveExisting.Text = "Remove Existing";
            this.toolTip1.SetToolTip(this.checkBoxMatchRemoveExisting, "Remove existing notes that overlap the new notes.");
            this.checkBoxMatchRemoveExisting.UseVisualStyleBackColor = true;
            // 
            // buttonNoteEditorCopyPatternPresetUpdate
            // 
            this.buttonNoteEditorCopyPatternPresetUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonNoteEditorCopyPatternPresetUpdate.ImageIndex = 30;
            this.buttonNoteEditorCopyPatternPresetUpdate.ImageList = this.imageList1;
            this.buttonNoteEditorCopyPatternPresetUpdate.Location = new System.Drawing.Point(68, 212);
            this.buttonNoteEditorCopyPatternPresetUpdate.Name = "buttonNoteEditorCopyPatternPresetUpdate";
            this.buttonNoteEditorCopyPatternPresetUpdate.Size = new System.Drawing.Size(24, 24);
            this.buttonNoteEditorCopyPatternPresetUpdate.TabIndex = 17;
            this.toolTip1.SetToolTip(this.buttonNoteEditorCopyPatternPresetUpdate, "Update Selected Preset");
            this.buttonNoteEditorCopyPatternPresetUpdate.UseVisualStyleBackColor = true;
            this.buttonNoteEditorCopyPatternPresetUpdate.Click += new System.EventHandler(this.buttonNoteEditorCopyPatternPresetUpdate_Click);
            // 
            // buttonNoteEditorCopyPatternPresetRemove
            // 
            this.buttonNoteEditorCopyPatternPresetRemove.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonNoteEditorCopyPatternPresetRemove.ImageIndex = 32;
            this.buttonNoteEditorCopyPatternPresetRemove.ImageList = this.imageList1;
            this.buttonNoteEditorCopyPatternPresetRemove.Location = new System.Drawing.Point(9, 212);
            this.buttonNoteEditorCopyPatternPresetRemove.Name = "buttonNoteEditorCopyPatternPresetRemove";
            this.buttonNoteEditorCopyPatternPresetRemove.Size = new System.Drawing.Size(24, 24);
            this.buttonNoteEditorCopyPatternPresetRemove.TabIndex = 16;
            this.toolTip1.SetToolTip(this.buttonNoteEditorCopyPatternPresetRemove, "Remove Selected Preset");
            this.buttonNoteEditorCopyPatternPresetRemove.UseVisualStyleBackColor = true;
            this.buttonNoteEditorCopyPatternPresetRemove.Click += new System.EventHandler(this.buttonNoteEditorCopyPatternPresetRemove_Click);
            // 
            // buttonNoteEditorCopyPatternPresetCreate
            // 
            this.buttonNoteEditorCopyPatternPresetCreate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonNoteEditorCopyPatternPresetCreate.ImageIndex = 35;
            this.buttonNoteEditorCopyPatternPresetCreate.ImageList = this.imageList1;
            this.buttonNoteEditorCopyPatternPresetCreate.Location = new System.Drawing.Point(98, 212);
            this.buttonNoteEditorCopyPatternPresetCreate.Name = "buttonNoteEditorCopyPatternPresetCreate";
            this.buttonNoteEditorCopyPatternPresetCreate.Size = new System.Drawing.Size(24, 24);
            this.buttonNoteEditorCopyPatternPresetCreate.TabIndex = 15;
            this.toolTip1.SetToolTip(this.buttonNoteEditorCopyPatternPresetCreate, "Create Preset from current configuration");
            this.buttonNoteEditorCopyPatternPresetCreate.UseVisualStyleBackColor = true;
            this.buttonNoteEditorCopyPatternPresetCreate.Click += new System.EventHandler(this.buttonNoteEditorCopyPatternPresetCreate_Click);
            // 
            // comboNoteEditorCopyPatternPreset
            // 
            this.comboNoteEditorCopyPatternPreset.FormattingEnabled = true;
            this.comboNoteEditorCopyPatternPreset.Location = new System.Drawing.Point(9, 188);
            this.comboNoteEditorCopyPatternPreset.Name = "comboNoteEditorCopyPatternPreset";
            this.comboNoteEditorCopyPatternPreset.Size = new System.Drawing.Size(113, 23);
            this.comboNoteEditorCopyPatternPreset.TabIndex = 14;
            this.comboNoteEditorCopyPatternPreset.SelectedIndexChanged += new System.EventHandler(this.comboNoteEditorCopyPatternPreset_SelectedIndexChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 174);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(47, 15);
            this.label48.TabIndex = 13;
            this.label48.Text = "Presets:";
            // 
            // button120
            // 
            this.button120.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button120.ImageIndex = 81;
            this.button120.ImageList = this.imageList1;
            this.button120.Location = new System.Drawing.Point(55, 19);
            this.button120.Name = "button120";
            this.button120.Size = new System.Drawing.Size(24, 24);
            this.button120.TabIndex = 11;
            this.toolTip1.SetToolTip(this.button120, "Find Prev");
            this.button120.UseVisualStyleBackColor = true;
            this.button120.Click += new System.EventHandler(this.buttonReplaceFindPrev_Click);
            // 
            // button121
            // 
            this.button121.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button121.ImageIndex = 80;
            this.button121.ImageList = this.imageList1;
            this.button121.Location = new System.Drawing.Point(78, 19);
            this.button121.Name = "button121";
            this.button121.Size = new System.Drawing.Size(24, 24);
            this.button121.TabIndex = 12;
            this.toolTip1.SetToolTip(this.button121, "Find Next");
            this.button121.UseVisualStyleBackColor = true;
            this.button121.Click += new System.EventHandler(this.buttonReplaceFindNext_Click);
            // 
            // checkBoxMatchForwardOnly
            // 
            this.checkBoxMatchForwardOnly.AutoSize = true;
            this.checkBoxMatchForwardOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchForwardOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchForwardOnly.Location = new System.Drawing.Point(9, 134);
            this.checkBoxMatchForwardOnly.Name = "checkBoxMatchForwardOnly";
            this.checkBoxMatchForwardOnly.Size = new System.Drawing.Size(80, 16);
            this.checkBoxMatchForwardOnly.TabIndex = 10;
            this.checkBoxMatchForwardOnly.Text = "Forward Only";
            this.toolTip1.SetToolTip(this.checkBoxMatchForwardOnly, "Find matches forward in time");
            this.checkBoxMatchForwardOnly.UseVisualStyleBackColor = true;
            // 
            // checkMatchBeat
            // 
            this.checkMatchBeat.AutoSize = true;
            this.checkMatchBeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkMatchBeat.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkMatchBeat.Location = new System.Drawing.Point(9, 101);
            this.checkMatchBeat.Name = "checkMatchBeat";
            this.checkMatchBeat.Size = new System.Drawing.Size(72, 16);
            this.checkMatchBeat.TabIndex = 9;
            this.checkMatchBeat.Text = "Match Beat";
            this.toolTip1.SetToolTip(this.checkMatchBeat, "Match similar beat times");
            this.checkMatchBeat.UseVisualStyleBackColor = true;
            // 
            // checkBoxKeepLengths
            // 
            this.checkBoxKeepLengths.AutoSize = true;
            this.checkBoxKeepLengths.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxKeepLengths.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxKeepLengths.Location = new System.Drawing.Point(9, 117);
            this.checkBoxKeepLengths.Name = "checkBoxKeepLengths";
            this.checkBoxKeepLengths.Size = new System.Drawing.Size(87, 16);
            this.checkBoxKeepLengths.TabIndex = 8;
            this.checkBoxKeepLengths.Text = "Keep Lengths 5";
            this.toolTip1.SetToolTip(this.checkBoxKeepLengths, "Set the new note lengths to the same length of the notes in the 5 button midi");
            this.checkBoxKeepLengths.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchSpacing
            // 
            this.checkBoxMatchSpacing.AutoSize = true;
            this.checkBoxMatchSpacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchSpacing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchSpacing.Location = new System.Drawing.Point(9, 83);
            this.checkBoxMatchSpacing.Name = "checkBoxMatchSpacing";
            this.checkBoxMatchSpacing.Size = new System.Drawing.Size(57, 16);
            this.checkBoxMatchSpacing.TabIndex = 7;
            this.checkBoxMatchSpacing.Text = "Spacing";
            this.toolTip1.SetToolTip(this.checkBoxMatchSpacing, "Match the spacing between notes in the selection");
            this.checkBoxMatchSpacing.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label20.Location = new System.Drawing.Point(9, 46);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 15);
            this.label20.TabIndex = 0;
            this.label20.Text = "Idle";
            // 
            // checkBoxFirstMatchOnly
            // 
            this.checkBoxFirstMatchOnly.AutoSize = true;
            this.checkBoxFirstMatchOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFirstMatchOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxFirstMatchOnly.Location = new System.Drawing.Point(68, 83);
            this.checkBoxFirstMatchOnly.Name = "checkBoxFirstMatchOnly";
            this.checkBoxFirstMatchOnly.Size = new System.Drawing.Size(64, 16);
            this.checkBoxFirstMatchOnly.TabIndex = 5;
            this.checkBoxFirstMatchOnly.Text = "First Only";
            this.toolTip1.SetToolTip(this.checkBoxFirstMatchOnly, "Only replace a single instance then stop");
            this.checkBoxFirstMatchOnly.UseVisualStyleBackColor = true;
            // 
            // button29
            // 
            this.button29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button29.ImageIndex = 34;
            this.button29.ImageList = this.imageList1;
            this.button29.Location = new System.Drawing.Point(6, 19);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(24, 24);
            this.button29.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button29, "Begin");
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // checkBoxMatchLength6
            // 
            this.checkBoxMatchLength6.AutoSize = true;
            this.checkBoxMatchLength6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchLength6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchLength6.Location = new System.Drawing.Point(68, 66);
            this.checkBoxMatchLength6.Name = "checkBoxMatchLength6";
            this.checkBoxMatchLength6.Size = new System.Drawing.Size(46, 16);
            this.checkBoxMatchLength6.TabIndex = 4;
            this.checkBoxMatchLength6.Text = "Len 6";
            this.toolTip1.SetToolTip(this.checkBoxMatchLength6, "Match using the lengths of the notes in the pro midi file");
            this.checkBoxMatchLength6.UseVisualStyleBackColor = true;
            // 
            // checkBoxMatchLengths
            // 
            this.checkBoxMatchLengths.AutoSize = true;
            this.checkBoxMatchLengths.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMatchLengths.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMatchLengths.Location = new System.Drawing.Point(9, 66);
            this.checkBoxMatchLengths.Name = "checkBoxMatchLengths";
            this.checkBoxMatchLengths.Size = new System.Drawing.Size(46, 16);
            this.checkBoxMatchLengths.TabIndex = 3;
            this.checkBoxMatchLengths.Text = "Len 5";
            this.toolTip1.SetToolTip(this.checkBoxMatchLengths, "Find Matching pattern using the length of the notes in the 5 button midi file");
            this.checkBoxMatchLengths.UseVisualStyleBackColor = true;
            // 
            // button28
            // 
            this.button28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button28.ImageIndex = 31;
            this.button28.ImageList = this.imageList1;
            this.button28.Location = new System.Drawing.Point(31, 19);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(24, 24);
            this.button28.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button28, "Cancel");
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button30
            // 
            this.button30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button30.ImageIndex = 29;
            this.button30.ImageList = this.imageList1;
            this.button30.Location = new System.Drawing.Point(102, 19);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(24, 24);
            this.button30.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button30, "Replaces notes matching the pattern selected");
            this.button30.UseVisualStyleBackColor = true;
            this.button30.Click += new System.EventHandler(this.button30_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Controls.Add(this.textBox10);
            this.groupBox2.Controls.Add(this.textBox11);
            this.groupBox2.Controls.Add(this.textBox12);
            this.groupBox2.Controls.Add(this.textBox13);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox2.Location = new System.Drawing.Point(52, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(48, 180);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Note";
            this.toolTip1.SetToolTip(this.groupBox2, "Note Fret");
            // 
            // textBox8
            // 
            this.textBox8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox8.Location = new System.Drawing.Point(9, 18);
            this.textBox8.MaxLength = 2;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(31, 23);
            this.textBox8.TabIndex = 0;
            this.textBox8.TextChanged += new System.EventHandler(this.onChangeString6);
            // 
            // textBox9
            // 
            this.textBox9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox9.Location = new System.Drawing.Point(9, 44);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(31, 23);
            this.textBox9.TabIndex = 1;
            this.textBox9.TextChanged += new System.EventHandler(this.onChangeString5);
            // 
            // textBox10
            // 
            this.textBox10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox10.Location = new System.Drawing.Point(9, 70);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(31, 23);
            this.textBox10.TabIndex = 2;
            this.textBox10.TextChanged += new System.EventHandler(this.onChangeString4);
            // 
            // textBox11
            // 
            this.textBox11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox11.Location = new System.Drawing.Point(9, 96);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(31, 23);
            this.textBox11.TabIndex = 3;
            this.textBox11.TextChanged += new System.EventHandler(this.onChangeString3);
            // 
            // textBox12
            // 
            this.textBox12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox12.Location = new System.Drawing.Point(9, 122);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(31, 23);
            this.textBox12.TabIndex = 4;
            this.textBox12.TextChanged += new System.EventHandler(this.onChangeString2);
            // 
            // textBox13
            // 
            this.textBox13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox13.Location = new System.Drawing.Point(9, 148);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(31, 23);
            this.textBox13.TabIndex = 5;
            this.textBox13.TextChanged += new System.EventHandler(this.onChangeString1);
            // 
            // groupBoxStrumMarkers
            // 
            this.groupBoxStrumMarkers.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxStrumMarkers.Controls.Add(this.buttonStrumNone);
            this.groupBoxStrumMarkers.Controls.Add(this.checkStrumLow);
            this.groupBoxStrumMarkers.Controls.Add(this.checkStrumMid);
            this.groupBoxStrumMarkers.Controls.Add(this.checkStrumHigh);
            this.groupBoxStrumMarkers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxStrumMarkers.Location = new System.Drawing.Point(233, 152);
            this.groupBoxStrumMarkers.Name = "groupBoxStrumMarkers";
            this.groupBoxStrumMarkers.Size = new System.Drawing.Size(106, 115);
            this.groupBoxStrumMarkers.TabIndex = 6;
            this.groupBoxStrumMarkers.TabStop = false;
            this.groupBoxStrumMarkers.Text = "Strum";
            // 
            // buttonStrumNone
            // 
            this.buttonStrumNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStrumNone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonStrumNone.ImageIndex = 32;
            this.buttonStrumNone.ImageList = this.imageList1;
            this.buttonStrumNone.Location = new System.Drawing.Point(6, 85);
            this.buttonStrumNone.Name = "buttonStrumNone";
            this.buttonStrumNone.Size = new System.Drawing.Size(30, 19);
            this.buttonStrumNone.TabIndex = 0;
            this.toolTip1.SetToolTip(this.buttonStrumNone, "Normal (Y)");
            this.buttonStrumNone.UseVisualStyleBackColor = true;
            this.buttonStrumNone.Click += new System.EventHandler(this.buttonStrumNone_Click);
            // 
            // checkStrumLow
            // 
            this.checkStrumLow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkStrumLow.Location = new System.Drawing.Point(6, 59);
            this.checkStrumLow.Name = "checkStrumLow";
            this.checkStrumLow.Size = new System.Drawing.Size(92, 20);
            this.checkStrumLow.TabIndex = 3;
            this.checkStrumLow.Text = "(L)ow";
            this.checkStrumLow.UseVisualStyleBackColor = true;
            // 
            // checkStrumMid
            // 
            this.checkStrumMid.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkStrumMid.Location = new System.Drawing.Point(6, 41);
            this.checkStrumMid.Name = "checkStrumMid";
            this.checkStrumMid.Size = new System.Drawing.Size(92, 19);
            this.checkStrumMid.TabIndex = 2;
            this.checkStrumMid.Text = "M( i )d";
            this.checkStrumMid.UseVisualStyleBackColor = true;
            // 
            // checkStrumHigh
            // 
            this.checkStrumHigh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkStrumHigh.Location = new System.Drawing.Point(6, 22);
            this.checkStrumHigh.Name = "checkStrumHigh";
            this.checkStrumHigh.Size = new System.Drawing.Size(92, 19);
            this.checkStrumHigh.TabIndex = 1;
            this.checkStrumHigh.Text = "High (U)";
            this.checkStrumHigh.UseVisualStyleBackColor = true;
            // 
            // tabModifierEditor
            // 
            this.tabModifierEditor.AutoScroll = true;
            this.tabModifierEditor.AutoScrollMinSize = new System.Drawing.Size(1010, 408);
            this.tabModifierEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabModifierEditor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabModifierEditor.Controls.Add(this.groupBoxSingleStringTremelo);
            this.tabModifierEditor.Controls.Add(this.groupBoxMultiStringTremelo);
            this.tabModifierEditor.Controls.Add(this.groupBoxArpeggio);
            this.tabModifierEditor.Controls.Add(this.groupBoxPowerup);
            this.tabModifierEditor.Controls.Add(this.groupBoxSolo);
            this.tabModifierEditor.ImageIndex = 71;
            this.tabModifierEditor.Location = new System.Drawing.Point(4, 23);
            this.tabModifierEditor.Name = "tabModifierEditor";
            this.tabModifierEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabModifierEditor.Size = new System.Drawing.Size(1095, 523);
            this.tabModifierEditor.TabIndex = 4;
            this.tabModifierEditor.Text = "Modifier Editor";
            this.tabModifierEditor.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // groupBoxSingleStringTremelo
            // 
            this.groupBoxSingleStringTremelo.Controls.Add(this.listBox5);
            this.groupBoxSingleStringTremelo.Controls.Add(this.button49);
            this.groupBoxSingleStringTremelo.Controls.Add(this.button48);
            this.groupBoxSingleStringTremelo.Controls.Add(this.button47);
            this.groupBoxSingleStringTremelo.Controls.Add(this.label18);
            this.groupBoxSingleStringTremelo.Controls.Add(this.button45);
            this.groupBoxSingleStringTremelo.Controls.Add(this.textBox18);
            this.groupBoxSingleStringTremelo.Controls.Add(this.button46);
            this.groupBoxSingleStringTremelo.Controls.Add(this.textBox17);
            this.groupBoxSingleStringTremelo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxSingleStringTremelo.Location = new System.Drawing.Point(772, 9);
            this.groupBoxSingleStringTremelo.Name = "groupBoxSingleStringTremelo";
            this.groupBoxSingleStringTremelo.Size = new System.Drawing.Size(186, 292);
            this.groupBoxSingleStringTremelo.TabIndex = 82;
            this.groupBoxSingleStringTremelo.TabStop = false;
            this.groupBoxSingleStringTremelo.Text = "Single String Tremelo";
            // 
            // listBox5
            // 
            this.listBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 15;
            this.listBox5.Location = new System.Drawing.Point(6, 19);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(174, 139);
            this.listBox5.TabIndex = 0;
            this.listBox5.SelectedIndexChanged += new System.EventHandler(this.listBox5_SelectedIndexChanged);
            // 
            // button49
            // 
            this.button49.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button49.ImageIndex = 36;
            this.button49.ImageList = this.imageList1;
            this.button49.Location = new System.Drawing.Point(156, 164);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(24, 24);
            this.button49.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button49, "Refresh - also unselects");
            this.button49.UseVisualStyleBackColor = true;
            this.button49.Click += new System.EventHandler(this.button49_Click);
            // 
            // button48
            // 
            this.button48.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button48.ImageIndex = 32;
            this.button48.ImageList = this.imageList1;
            this.button48.Location = new System.Drawing.Point(6, 164);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(24, 24);
            this.button48.TabIndex = 1;
            this.button48.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button48, "Delete");
            this.button48.UseVisualStyleBackColor = true;
            this.button48.Click += new System.EventHandler(this.button48_Click);
            // 
            // button47
            // 
            this.button47.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button47.ImageIndex = 35;
            this.button47.ImageList = this.imageList1;
            this.button47.Location = new System.Drawing.Point(126, 164);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(24, 24);
            this.button47.TabIndex = 6;
            this.button47.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button47, "Create New Single String Tremelo (T)");
            this.button47.UseVisualStyleBackColor = true;
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(6, 230);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(26, 15);
            this.label18.TabIndex = 72;
            this.label18.Text = "Idle";
            // 
            // button45
            // 
            this.button45.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button45.ImageIndex = 31;
            this.button45.ImageList = this.imageList1;
            this.button45.Location = new System.Drawing.Point(96, 164);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(24, 24);
            this.button45.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button45, "Cancel");
            this.button45.UseVisualStyleBackColor = true;
            this.button45.Click += new System.EventHandler(this.button45_Click);
            // 
            // textBox18
            // 
            this.textBox18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox18.Location = new System.Drawing.Point(95, 196);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(85, 23);
            this.textBox18.TabIndex = 4;
            // 
            // button46
            // 
            this.button46.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button46.ImageIndex = 43;
            this.button46.ImageList = this.imageList1;
            this.button46.Location = new System.Drawing.Point(156, 225);
            this.button46.Name = "button46";
            this.button46.Size = new System.Drawing.Size(24, 24);
            this.button46.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button46, "Update Selected");
            this.button46.UseVisualStyleBackColor = true;
            this.button46.Click += new System.EventHandler(this.button46_Click);
            // 
            // textBox17
            // 
            this.textBox17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox17.Location = new System.Drawing.Point(6, 196);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(85, 23);
            this.textBox17.TabIndex = 3;
            // 
            // groupBoxMultiStringTremelo
            // 
            this.groupBoxMultiStringTremelo.Controls.Add(this.listBox4);
            this.groupBoxMultiStringTremelo.Controls.Add(this.button43);
            this.groupBoxMultiStringTremelo.Controls.Add(this.button42);
            this.groupBoxMultiStringTremelo.Controls.Add(this.button41);
            this.groupBoxMultiStringTremelo.Controls.Add(this.label16);
            this.groupBoxMultiStringTremelo.Controls.Add(this.textBox16);
            this.groupBoxMultiStringTremelo.Controls.Add(this.textBox15);
            this.groupBoxMultiStringTremelo.Controls.Add(this.button40);
            this.groupBoxMultiStringTremelo.Controls.Add(this.button39);
            this.groupBoxMultiStringTremelo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxMultiStringTremelo.Location = new System.Drawing.Point(581, 9);
            this.groupBoxMultiStringTremelo.Name = "groupBoxMultiStringTremelo";
            this.groupBoxMultiStringTremelo.Size = new System.Drawing.Size(186, 292);
            this.groupBoxMultiStringTremelo.TabIndex = 81;
            this.groupBoxMultiStringTremelo.TabStop = false;
            this.groupBoxMultiStringTremelo.Text = "Multi String Tremelo";
            // 
            // listBox4
            // 
            this.listBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 15;
            this.listBox4.Location = new System.Drawing.Point(6, 19);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(174, 139);
            this.listBox4.TabIndex = 0;
            this.listBox4.SelectedIndexChanged += new System.EventHandler(this.listBox4_SelectedIndexChanged);
            // 
            // button43
            // 
            this.button43.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button43.ImageIndex = 36;
            this.button43.ImageList = this.imageList1;
            this.button43.Location = new System.Drawing.Point(156, 164);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(24, 24);
            this.button43.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button43, "Refresh - also unselects");
            this.button43.UseVisualStyleBackColor = true;
            this.button43.Click += new System.EventHandler(this.button43_Click);
            // 
            // button42
            // 
            this.button42.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button42.ImageIndex = 32;
            this.button42.ImageList = this.imageList1;
            this.button42.Location = new System.Drawing.Point(6, 164);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(24, 24);
            this.button42.TabIndex = 1;
            this.button42.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button42, "Delete");
            this.button42.UseVisualStyleBackColor = true;
            this.button42.Click += new System.EventHandler(this.button42_Click);
            // 
            // button41
            // 
            this.button41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button41.ImageIndex = 35;
            this.button41.ImageList = this.imageList1;
            this.button41.Location = new System.Drawing.Point(126, 164);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(24, 24);
            this.button41.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button41, "Create New Multi String Tremelo (M)");
            this.button41.UseVisualStyleBackColor = true;
            this.button41.Click += new System.EventHandler(this.button41_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(6, 230);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(26, 15);
            this.label16.TabIndex = 62;
            this.label16.Text = "Idle";
            // 
            // textBox16
            // 
            this.textBox16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox16.Location = new System.Drawing.Point(95, 196);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(85, 23);
            this.textBox16.TabIndex = 4;
            this.textBox16.TextChanged += new System.EventHandler(this.textBox16_TextChanged);
            // 
            // textBox15
            // 
            this.textBox15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox15.Location = new System.Drawing.Point(6, 196);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(85, 23);
            this.textBox15.TabIndex = 3;
            this.textBox15.TextChanged += new System.EventHandler(this.textBox15_TextChanged);
            // 
            // button40
            // 
            this.button40.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button40.ImageIndex = 43;
            this.button40.ImageList = this.imageList1;
            this.button40.Location = new System.Drawing.Point(156, 225);
            this.button40.Name = "button40";
            this.button40.Size = new System.Drawing.Size(24, 24);
            this.button40.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button40, "Update Selected");
            this.button40.UseVisualStyleBackColor = true;
            this.button40.Click += new System.EventHandler(this.button40_Click);
            // 
            // button39
            // 
            this.button39.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button39.ImageIndex = 31;
            this.button39.ImageList = this.imageList1;
            this.button39.Location = new System.Drawing.Point(96, 164);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(24, 24);
            this.button39.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button39, "Cancel");
            this.button39.UseVisualStyleBackColor = true;
            this.button39.Click += new System.EventHandler(this.button39_Click);
            // 
            // groupBoxArpeggio
            // 
            this.groupBoxArpeggio.Controls.Add(this.listBox3);
            this.groupBoxArpeggio.Controls.Add(this.button26);
            this.groupBoxArpeggio.Controls.Add(this.button25);
            this.groupBoxArpeggio.Controls.Add(this.button24);
            this.groupBoxArpeggio.Controls.Add(this.label13);
            this.groupBoxArpeggio.Controls.Add(this.button58);
            this.groupBoxArpeggio.Controls.Add(this.textBox30);
            this.groupBoxArpeggio.Controls.Add(this.textBox29);
            this.groupBoxArpeggio.Controls.Add(this.button23);
            this.groupBoxArpeggio.Controls.Add(this.button22);
            this.groupBoxArpeggio.Controls.Add(this.checkBoxCreateArpeggioHelperNotes);
            this.groupBoxArpeggio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxArpeggio.Location = new System.Drawing.Point(390, 9);
            this.groupBoxArpeggio.Name = "groupBoxArpeggio";
            this.groupBoxArpeggio.Size = new System.Drawing.Size(186, 292);
            this.groupBoxArpeggio.TabIndex = 80;
            this.groupBoxArpeggio.TabStop = false;
            this.groupBoxArpeggio.Text = "Arpeggios";
            // 
            // listBox3
            // 
            this.listBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 15;
            this.listBox3.Location = new System.Drawing.Point(6, 19);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(174, 139);
            this.listBox3.TabIndex = 0;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // button26
            // 
            this.button26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button26.ImageIndex = 36;
            this.button26.ImageList = this.imageList1;
            this.button26.Location = new System.Drawing.Point(156, 164);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(24, 24);
            this.button26.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button26, "Refresh - also unselects");
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button25
            // 
            this.button25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button25.ImageIndex = 32;
            this.button25.ImageList = this.imageList1;
            this.button25.Location = new System.Drawing.Point(6, 164);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(24, 24);
            this.button25.TabIndex = 1;
            this.button25.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button25, "Delete");
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button24
            // 
            this.button24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button24.ImageIndex = 35;
            this.button24.ImageList = this.imageList1;
            this.button24.Location = new System.Drawing.Point(126, 164);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(24, 24);
            this.button24.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button24, "Create New Arpeggio (A)");
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button24_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(6, 230);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(26, 15);
            this.label13.TabIndex = 51;
            this.label13.Text = "Idle";
            // 
            // button58
            // 
            this.button58.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button58.ImageIndex = 64;
            this.button58.ImageList = this.imageList1;
            this.button58.Location = new System.Drawing.Point(156, 262);
            this.button58.Name = "button58";
            this.button58.Size = new System.Drawing.Size(24, 24);
            this.button58.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button58, "Remove All");
            this.button58.UseVisualStyleBackColor = true;
            this.button58.Click += new System.EventHandler(this.button58_Click);
            // 
            // textBox30
            // 
            this.textBox30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox30.Location = new System.Drawing.Point(95, 196);
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new System.Drawing.Size(85, 23);
            this.textBox30.TabIndex = 4;
            this.textBox30.TextChanged += new System.EventHandler(this.textBox30_TextChanged);
            // 
            // textBox29
            // 
            this.textBox29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox29.Location = new System.Drawing.Point(6, 196);
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new System.Drawing.Size(85, 23);
            this.textBox29.TabIndex = 3;
            this.textBox29.TextChanged += new System.EventHandler(this.textBox29_TextChanged);
            // 
            // button23
            // 
            this.button23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button23.ImageIndex = 43;
            this.button23.ImageList = this.imageList1;
            this.button23.Location = new System.Drawing.Point(156, 225);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(24, 24);
            this.button23.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button23, "Update Selected");
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button23_Click);
            // 
            // button22
            // 
            this.button22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button22.ImageIndex = 31;
            this.button22.ImageList = this.imageList1;
            this.button22.Location = new System.Drawing.Point(97, 164);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(24, 24);
            this.button22.TabIndex = 8;
            this.toolTip1.SetToolTip(this.button22, "Cancel");
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // checkBoxCreateArpeggioHelperNotes
            // 
            this.checkBoxCreateArpeggioHelperNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxCreateArpeggioHelperNotes.Location = new System.Drawing.Point(6, 266);
            this.checkBoxCreateArpeggioHelperNotes.Name = "checkBoxCreateArpeggioHelperNotes";
            this.checkBoxCreateArpeggioHelperNotes.Size = new System.Drawing.Size(144, 19);
            this.checkBoxCreateArpeggioHelperNotes.TabIndex = 9;
            this.checkBoxCreateArpeggioHelperNotes.Text = "Add (G)host Notes";
            this.checkBoxCreateArpeggioHelperNotes.UseVisualStyleBackColor = true;
            // 
            // groupBoxPowerup
            // 
            this.groupBoxPowerup.Controls.Add(this.listBox2);
            this.groupBoxPowerup.Controls.Add(this.button21);
            this.groupBoxPowerup.Controls.Add(this.button20);
            this.groupBoxPowerup.Controls.Add(this.button19);
            this.groupBoxPowerup.Controls.Add(this.label10);
            this.groupBoxPowerup.Controls.Add(this.textBox28);
            this.groupBoxPowerup.Controls.Add(this.textBox27);
            this.groupBoxPowerup.Controls.Add(this.button18);
            this.groupBoxPowerup.Controls.Add(this.button17);
            this.groupBoxPowerup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxPowerup.Location = new System.Drawing.Point(199, 9);
            this.groupBoxPowerup.Name = "groupBoxPowerup";
            this.groupBoxPowerup.Size = new System.Drawing.Size(186, 292);
            this.groupBoxPowerup.TabIndex = 79;
            this.groupBoxPowerup.TabStop = false;
            this.groupBoxPowerup.Text = "Powerups";
            this.groupBoxPowerup.Enter += new System.EventHandler(this.groupBoxPowerup_Enter);
            // 
            // listBox2
            // 
            this.listBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(6, 19);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(174, 139);
            this.listBox2.TabIndex = 0;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // button21
            // 
            this.button21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button21.ImageIndex = 36;
            this.button21.ImageList = this.imageList1;
            this.button21.Location = new System.Drawing.Point(156, 164);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(24, 24);
            this.button21.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button21, "Refresh - also unselects");
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button20
            // 
            this.button20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button20.ImageIndex = 32;
            this.button20.ImageList = this.imageList1;
            this.button20.Location = new System.Drawing.Point(6, 164);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(24, 24);
            this.button20.TabIndex = 1;
            this.button20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button20, "Delete");
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // button19
            // 
            this.button19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button19.ImageIndex = 35;
            this.button19.ImageList = this.imageList1;
            this.button19.Location = new System.Drawing.Point(126, 164);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(24, 24);
            this.button19.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button19, "Create New Powerup (P)");
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(6, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 15);
            this.label10.TabIndex = 41;
            this.label10.Text = "Idle";
            // 
            // textBox28
            // 
            this.textBox28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox28.Location = new System.Drawing.Point(95, 196);
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new System.Drawing.Size(85, 23);
            this.textBox28.TabIndex = 4;
            this.textBox28.TextChanged += new System.EventHandler(this.textBox28_TextChanged);
            // 
            // textBox27
            // 
            this.textBox27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox27.Location = new System.Drawing.Point(6, 196);
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new System.Drawing.Size(85, 23);
            this.textBox27.TabIndex = 3;
            this.textBox27.TextChanged += new System.EventHandler(this.textBox27_TextChanged);
            // 
            // button18
            // 
            this.button18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button18.ImageIndex = 43;
            this.button18.ImageList = this.imageList1;
            this.button18.Location = new System.Drawing.Point(156, 225);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(24, 24);
            this.button18.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button18, "Update Selected");
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button17
            // 
            this.button17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button17.ImageIndex = 31;
            this.button17.ImageList = this.imageList1;
            this.button17.Location = new System.Drawing.Point(96, 164);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(24, 24);
            this.button17.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button17, "Cancel");
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // groupBoxSolo
            // 
            this.groupBoxSolo.Controls.Add(this.listBoxSolos);
            this.groupBoxSolo.Controls.Add(this.button12);
            this.groupBoxSolo.Controls.Add(this.button13);
            this.groupBoxSolo.Controls.Add(this.button14);
            this.groupBoxSolo.Controls.Add(this.label11);
            this.groupBoxSolo.Controls.Add(this.textBox1);
            this.groupBoxSolo.Controls.Add(this.textBox26);
            this.groupBoxSolo.Controls.Add(this.button15);
            this.groupBoxSolo.Controls.Add(this.button16);
            this.groupBoxSolo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxSolo.Location = new System.Drawing.Point(8, 9);
            this.groupBoxSolo.Name = "groupBoxSolo";
            this.groupBoxSolo.Size = new System.Drawing.Size(186, 292);
            this.groupBoxSolo.TabIndex = 78;
            this.groupBoxSolo.TabStop = false;
            this.groupBoxSolo.Text = "Solos";
            // 
            // listBoxSolos
            // 
            this.listBoxSolos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBoxSolos.FormattingEnabled = true;
            this.listBoxSolos.ItemHeight = 15;
            this.listBoxSolos.Location = new System.Drawing.Point(6, 19);
            this.listBoxSolos.Name = "listBoxSolos";
            this.listBoxSolos.Size = new System.Drawing.Size(174, 139);
            this.listBoxSolos.TabIndex = 0;
            this.listBoxSolos.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button12
            // 
            this.button12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button12.ImageIndex = 36;
            this.button12.ImageList = this.imageList1;
            this.button12.Location = new System.Drawing.Point(156, 164);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(24, 24);
            this.button12.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button12, "Refresh - also unselects");
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button13.ImageIndex = 32;
            this.button13.ImageList = this.imageList1;
            this.button13.Location = new System.Drawing.Point(6, 164);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(24, 24);
            this.button13.TabIndex = 1;
            this.button13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button13, "Delete");
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button14.ImageIndex = 35;
            this.button14.ImageList = this.imageList1;
            this.button14.Location = new System.Drawing.Point(126, 164);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(24, 24);
            this.button14.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button14, "Create New Solo (S)");
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(6, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 15);
            this.label11.TabIndex = 31;
            this.label11.Text = "Idle";
            // 
            // textBox1
            // 
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox1.Location = new System.Drawing.Point(6, 196);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(85, 23);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox26
            // 
            this.textBox26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox26.Location = new System.Drawing.Point(95, 196);
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new System.Drawing.Size(85, 23);
            this.textBox26.TabIndex = 4;
            this.textBox26.TextChanged += new System.EventHandler(this.textBox26_TextChanged);
            // 
            // button15
            // 
            this.button15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button15.ImageIndex = 43;
            this.button15.ImageList = this.imageList1;
            this.button15.Location = new System.Drawing.Point(156, 225);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(24, 24);
            this.button15.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button15, "Update Selected");
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button16.ImageIndex = 31;
            this.button16.ImageList = this.imageList1;
            this.button16.Location = new System.Drawing.Point(96, 164);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(24, 24);
            this.button16.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button16, "Cancel");
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.groupBox43);
            this.tabPageEvents.Controls.Add(this.groupBoxTextEvents);
            this.tabPageEvents.Controls.Add(this.groupBoxProBassTrainers);
            this.tabPageEvents.Controls.Add(this.groupBoxProGuitarTrainers);
            this.tabPageEvents.ImageIndex = 59;
            this.tabPageEvents.Location = new System.Drawing.Point(4, 23);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(1095, 523);
            this.tabPageEvents.TabIndex = 10;
            this.tabPageEvents.Text = "Events";
            // 
            // groupBox43
            // 
            this.groupBox43.Controls.Add(this.buttonRefresh108Events);
            this.groupBox43.Controls.Add(this.checkBoxShow108);
            this.groupBox43.Controls.Add(this.comboBox180);
            this.groupBox43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox43.Location = new System.Drawing.Point(612, 6);
            this.groupBox43.Name = "groupBox43";
            this.groupBox43.Size = new System.Drawing.Size(265, 333);
            this.groupBox43.TabIndex = 3;
            this.groupBox43.TabStop = false;
            this.groupBox43.Text = "Hand Position Events";
            // 
            // buttonRefresh108Events
            // 
            this.buttonRefresh108Events.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRefresh108Events.ImageIndex = 36;
            this.buttonRefresh108Events.ImageList = this.imageList1;
            this.buttonRefresh108Events.Location = new System.Drawing.Point(235, 295);
            this.buttonRefresh108Events.Name = "buttonRefresh108Events";
            this.buttonRefresh108Events.Size = new System.Drawing.Size(24, 24);
            this.buttonRefresh108Events.TabIndex = 24;
            this.toolTip1.SetToolTip(this.buttonRefresh108Events, "Refresh - Also Unselects");
            this.buttonRefresh108Events.UseVisualStyleBackColor = true;
            this.buttonRefresh108Events.Click += new System.EventHandler(this.buttonRefresh108Events_Click);
            // 
            // checkBoxShow108
            // 
            this.checkBoxShow108.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxShow108.Location = new System.Drawing.Point(6, 299);
            this.checkBoxShow108.Name = "checkBoxShow108";
            this.checkBoxShow108.Size = new System.Drawing.Size(102, 19);
            this.checkBoxShow108.TabIndex = 23;
            this.checkBoxShow108.Text = "Show Events";
            this.checkBoxShow108.UseVisualStyleBackColor = true;
            this.checkBoxShow108.CheckedChanged += new System.EventHandler(this.checkBoxShow108_CheckedChanged);
            // 
            // comboBox180
            // 
            this.comboBox180.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox180.FormattingEnabled = true;
            this.comboBox180.ItemHeight = 15;
            this.comboBox180.Location = new System.Drawing.Point(6, 19);
            this.comboBox180.Name = "comboBox180";
            this.comboBox180.Size = new System.Drawing.Size(253, 274);
            this.comboBox180.TabIndex = 1;
            this.comboBox180.SelectedIndexChanged += new System.EventHandler(this.comboBox180_SelectedIndexChanged);
            // 
            // groupBoxTextEvents
            // 
            this.groupBoxTextEvents.Controls.Add(this.buttonAddTextEvent);
            this.groupBoxTextEvents.Controls.Add(this.checkBoxShowTrainersInTextEvents);
            this.groupBoxTextEvents.Controls.Add(this.listTextEvents);
            this.groupBoxTextEvents.Controls.Add(this.buttonRefreshTextEvents);
            this.groupBoxTextEvents.Controls.Add(this.buttonDeleteTextEvent);
            this.groupBoxTextEvents.Controls.Add(this.textBoxEventTick);
            this.groupBoxTextEvents.Controls.Add(this.textBoxEventText);
            this.groupBoxTextEvents.Controls.Add(this.buttonUpdateTextEvent);
            this.groupBoxTextEvents.Controls.Add(this.label6);
            this.groupBoxTextEvents.Controls.Add(this.label7);
            this.groupBoxTextEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxTextEvents.Location = new System.Drawing.Point(392, 6);
            this.groupBoxTextEvents.Name = "groupBoxTextEvents";
            this.groupBoxTextEvents.Size = new System.Drawing.Size(214, 333);
            this.groupBoxTextEvents.TabIndex = 2;
            this.groupBoxTextEvents.TabStop = false;
            this.groupBoxTextEvents.Text = "Text Events";
            // 
            // buttonAddTextEvent
            // 
            this.buttonAddTextEvent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonAddTextEvent.ImageIndex = 35;
            this.buttonAddTextEvent.ImageList = this.imageList1;
            this.buttonAddTextEvent.Location = new System.Drawing.Point(151, 268);
            this.buttonAddTextEvent.Name = "buttonAddTextEvent";
            this.buttonAddTextEvent.Size = new System.Drawing.Size(24, 24);
            this.buttonAddTextEvent.TabIndex = 8;
            this.toolTip1.SetToolTip(this.buttonAddTextEvent, "Create New Text Event");
            this.buttonAddTextEvent.UseVisualStyleBackColor = true;
            this.buttonAddTextEvent.Click += new System.EventHandler(this.buttonAddTextEvent_Click_1);
            // 
            // checkBoxShowTrainersInTextEvents
            // 
            this.checkBoxShowTrainersInTextEvents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxShowTrainersInTextEvents.Location = new System.Drawing.Point(9, 272);
            this.checkBoxShowTrainersInTextEvents.Name = "checkBoxShowTrainersInTextEvents";
            this.checkBoxShowTrainersInTextEvents.Size = new System.Drawing.Size(110, 19);
            this.checkBoxShowTrainersInTextEvents.TabIndex = 7;
            this.checkBoxShowTrainersInTextEvents.Text = "Show Trainers";
            this.checkBoxShowTrainersInTextEvents.UseVisualStyleBackColor = true;
            this.checkBoxShowTrainersInTextEvents.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // listTextEvents
            // 
            this.listTextEvents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listTextEvents.FormattingEnabled = true;
            this.listTextEvents.ItemHeight = 15;
            this.listTextEvents.Location = new System.Drawing.Point(6, 19);
            this.listTextEvents.Name = "listTextEvents";
            this.listTextEvents.Size = new System.Drawing.Size(199, 169);
            this.listTextEvents.TabIndex = 0;
            this.listTextEvents.SelectedIndexChanged += new System.EventHandler(this.listTextEvents_SelectedIndexChanged);
            // 
            // buttonRefreshTextEvents
            // 
            this.buttonRefreshTextEvents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRefreshTextEvents.ImageIndex = 36;
            this.buttonRefreshTextEvents.ImageList = this.imageList1;
            this.buttonRefreshTextEvents.Location = new System.Drawing.Point(181, 194);
            this.buttonRefreshTextEvents.Name = "buttonRefreshTextEvents";
            this.buttonRefreshTextEvents.Size = new System.Drawing.Size(24, 24);
            this.buttonRefreshTextEvents.TabIndex = 4;
            this.toolTip1.SetToolTip(this.buttonRefreshTextEvents, "Refresh - Also Unselects");
            this.buttonRefreshTextEvents.UseVisualStyleBackColor = true;
            this.buttonRefreshTextEvents.Click += new System.EventHandler(this.buttonRefreshTextEvents_Click);
            // 
            // buttonDeleteTextEvent
            // 
            this.buttonDeleteTextEvent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDeleteTextEvent.ImageIndex = 32;
            this.buttonDeleteTextEvent.ImageList = this.imageList1;
            this.buttonDeleteTextEvent.Location = new System.Drawing.Point(151, 194);
            this.buttonDeleteTextEvent.Name = "buttonDeleteTextEvent";
            this.buttonDeleteTextEvent.Size = new System.Drawing.Size(24, 24);
            this.buttonDeleteTextEvent.TabIndex = 3;
            this.buttonDeleteTextEvent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonDeleteTextEvent, "Delete Selected Event");
            this.buttonDeleteTextEvent.UseVisualStyleBackColor = true;
            this.buttonDeleteTextEvent.Click += new System.EventHandler(this.buttonDeleteTextEvent_Click);
            // 
            // textBoxEventTick
            // 
            this.textBoxEventTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxEventTick.Location = new System.Drawing.Point(43, 196);
            this.textBoxEventTick.Name = "textBoxEventTick";
            this.textBoxEventTick.Size = new System.Drawing.Size(102, 23);
            this.textBoxEventTick.TabIndex = 2;
            // 
            // textBoxEventText
            // 
            this.textBoxEventText.AutoCompleteCustomSource.AddRange(new string[] {
            "[bonusfx]",
            "[bonusfx_optional]",
            "[chorus]",
            "[chrd0  ]",
            "[chrd0 Ab5]",
            "[chrd0 Bb]",
            "[chrd0 Bb5]",
            "[chrd0 C5]",
            "[chrd0 Cmin]",
            "[chrd0 Dmin]",
            "[chrd0 Eb/Bb]",
            "[chrd0 Eb/G]",
            "[chrd0 Eb]",
            "[chrd0 Eb5/Bb]",
            "[chrd0 Eb5]",
            "[chrd0 F5]",
            "[chrd1  ]",
            "[chrd1 Ab5]",
            "[chrd1 Bb]",
            "[chrd1 Bb5]",
            "[chrd1 C5]",
            "[chrd1 Cmin]",
            "[chrd1 D]",
            "[chrd1 D5]",
            "[chrd1 Dmin]",
            "[chrd1 E5]",
            "[chrd1 Eb/Bb]",
            "[chrd1 Eb/G]",
            "[chrd1 Eb]",
            "[chrd1 Eb5/Bb]",
            "[chrd1 Eb5]",
            "[chrd1 F]",
            "[chrd1 F5]",
            "[chrd1 G5]",
            "[chrd2  ]",
            "[chrd2 A#min]",
            "[chrd2 A]",
            "[chrd2 A5]",
            "[chrd2 Ab5]",
            "[chrd2 Asus4]",
            "[chrd2 B(b5)]",
            "[chrd2 B]",
            "[chrd2 B5]",
            "[chrd2 Bb]",
            "[chrd2 Bb5]",
            "[chrd2 Bend]",
            "[chrd2 C#5]",
            "[chrd2 C5/G]",
            "[chrd2 C5]",
            "[chrd2 Cmin]",
            "[chrd2 D<gtr>4</gtr>]",
            "[chrd2 D5/A]",
            "[chrd2 D5]",
            "[chrd2 Dmin]",
            "[chrd2 Dsus4]",
            "[chrd2 E/G#]",
            "[chrd2 E]",
            "[chrd2 E5/B]",
            "[chrd2 E5]",
            "[chrd2 E5add7]",
            "[chrd2 E7]",
            "[chrd2 E7sus4]",
            "[chrd2 Eb/Bb]",
            "[chrd2 Eb/G]",
            "[chrd2 Eb]",
            "[chrd2 Eb5/Bb]",
            "[chrd2 Eb5]",
            "[chrd2 Em]",
            "[chrd2 F#5/C#]",
            "[chrd2 F#5]",
            "[chrd2 F#min]",
            "[chrd2 F]",
            "[chrd2 F5/C]",
            "[chrd2 F5]",
            "[chrd2 G]",
            "[chrd2 G5]",
            "[chrd3  ]",
            "[chrd3 A#min]",
            "[chrd3 A]",
            "[chrd3 A5]",
            "[chrd3 Ab5]",
            "[chrd3 Asus4]",
            "[chrd3 B]",
            "[chrd3 B5/F#]",
            "[chrd3 B7]",
            "[chrd3 Bb]",
            "[chrd3 Bb5]",
            "[chrd3 Bend]",
            "[chrd3 C5/G]",
            "[chrd3 Cmaj]",
            "[chrd3 Cmin]",
            "[chrd3 D5/A]",
            "[chrd3 Dmin]",
            "[chrd3 E]",
            "[chrd3 E5/B]",
            "[chrd3 Eb/Bb]",
            "[chrd3 Eb/G]",
            "[chrd3 Eb]",
            "[chrd3 Eb5/Bb]",
            "[chrd3 Eb5]",
            "[chrd3 F#5/C#]",
            "[chrd3 F#min]",
            "[chrd3 F5/C]",
            "[clap_end]",
            "[clap_start]",
            "[coda]",
            "[coop_all_behind]",
            "[coop_all_far]",
            "[coop_all_near]",
            "[coop_b_behind]",
            "[coop_b_near]",
            "[coop_bg_near]",
            "[coop_bv_near]",
            "[coop_d_behind]",
            "[coop_d_near]",
            "[coop_front_near]",
            "[coop_g_behind]",
            "[coop_g_closeup_hand]",
            "[coop_g_closeup_head]",
            "[coop_g_near]",
            "[coop_gv_behind]",
            "[coop_gv_near]",
            "[coop_v_behind]",
            "[coop_v_closeup]",
            "[coop_v_near]",
            "[corwd_normal]",
            "[cowbell_end]",
            "[cowbell_start]",
            "[crowd_clap]",
            "[crowd_fists_off]",
            "[crowd_fists_on]",
            "[crowd_horns_off]",
            "[crowd_horns_on]",
            "[crowd_intense]",
            "[crowd_intsense]",
            "[crowd_lighters_off]",
            "[crowd_lighters_on]",
            "[crowd_mello]",
            "[crowd_mellow]",
            "[crowd_noclap]",
            "[crowd_normal]",
            "[crowd_notclap]",
            "[crowd_realtime]",
            "[directed_all]",
            "[directed_all_cam]",
            "[directed_duo_gb]",
            "[directed_guitar]",
            "[directed_guitar_cls]",
            "[do_directed_cut directed_all]",
            "[do_directed_cut directed_all_cam]",
            "[do_directed_cut directed_all_lt]",
            "[do_directed_cut directed_all_yeah]",
            "[do_directed_cut directed_bass]",
            "[do_directed_cut directed_bass]",
            "[do_directed_cut directed_bass_cam]",
            "[do_directed_cut directed_bass_cls]",
            "[do_directed_cut directed_bass_np]",
            "[do_directed_cut directed_bre]",
            "[do_directed_cut directed_bre]",
            "[do_directed_cut directed_brej]",
            "[do_directed_cut directed_brej]",
            "[do_directed_cut directed_crowd_b]",
            "[do_directed_cut directed_crowd_g]",
            "[do_directed_cut directed_drums]",
            "[do_directed_cut directed_drums]",
            "[do_directed_cut directed_drums_kd]",
            "[do_directed_cut directed_drums_lt]",
            "[do_directed_cut directed_drums_np]",
            "[do_directed_cut directed_drums_pnt]",
            "[do_directed_cut directed_duo_bass]",
            "[do_directed_cut directed_duo_drums]",
            "[do_directed_cut directed_duo_gb]",
            "[do_directed_cut directed_duo_guitar]",
            "[do_directed_cut directed_guitar]",
            "[do_directed_cut directed_guitar_cam]",
            "[do_directed_cut directed_guitar_cls]",
            "[do_directed_cut directed_guitar_np]",
            "[do_directed_cut directed_stagedive]",
            "[do_directed_cut directed_vocals]",
            "[do_directed_cut directed_vocals_cam]",
            "[do_directed_cut directed_vocals_cls]",
            "[do_directed_cut directed_vocals_np]",
            "[do_optional_cut directed_all]",
            "[end]",
            "[FogOff]",
            "[FogOn]",
            "[FogOn]",
            "[gtr_break]",
            "[idle]",
            "[idle_intense]",
            "[idle_realtime]",
            "[intense]",
            "[lighting ()]",
            "[lighting (blackout_fast)]",
            "[lighting (blackout_slow)]",
            "[lighting (blackout_spot)]",
            "[lighting (bre)]",
            "[lighting (chorus)]",
            "[lighting (dischord)]",
            "[lighting (flare_fast)]",
            "[lighting (flare_slow)]",
            "[lighting (frenzy)]",
            "[lighting (harmony)]",
            "[lighting (intro)]",
            "[lighting (loop_cool)]",
            "[lighting (loop_warm)]",
            "[lighting (manual_cool)]",
            "[lighting (manual_warm)]",
            "[lighting (searchlights)]",
            "[lighting (silhouettes)]",
            "[lighting (silhouettes_spot)]",
            "[lighting (stomp)]",
            "[lighting (strobe_fast)]",
            "[lighting (strobe_slow)]",
            "[lighting (sweep)]",
            "[lighting (verse)]",
            "[lighting(silhouettes_spot)]",
            "[map DropD2]",
            "[map HandMap_AllBend]",
            "[map Handmap_AllChords]",
            "[map HandMap_C]",
            "[map HandMap_Chord_A]",
            "[map HandMap_Chord_C]",
            "[map HandMap_Chord_D]",
            "[map HandMap_Chord_Default]",
            "[map HandMap_D]",
            "[map handMap_Default]",
            "[map HandMap_DropD]",
            "[map HandMap_DropD2]",
            "[map HandMap_NoChords]",
            "[map HandMap_Solo]",
            "[map StrumMap_Default]",
            "[map StrumMap_Pick]",
            "[map StrumMap_SlapBass]",
            "[mellow]",
            "[mix 0 drums0]",
            "[mix 0 drums1]",
            "[mix 0 drums1d]",
            "[mix 0 drums1easy]",
            "[mix 0 drums1easynokick]",
            "[mix 0 drums2]",
            "[mix 0 drums2d]",
            "[mix 0 drums2easy]",
            "[mix 0 drums2easynokick]",
            "[mix 0 drums3]",
            "[mix 0 drums3d]",
            "[mix 0 drums3easy]",
            "[mix 0 drums3easynokick]",
            "[mix 0 drums4]",
            "[mix 0 drums4easynokick]",
            "[mix 1 drums0]",
            "[mix 1 drums1]",
            "[mix 1 drums1easynokick]",
            "[mix 1 drums2]",
            "[mix 1 drums2d]",
            "[mix 1 drums2easynokick]",
            "[mix 1 drums3]",
            "[mix 1 drums3d]",
            "[mix 1 drums3easy]",
            "[mix 1 drums3easynokick]",
            "[mix 1 drums4]",
            "[mix 1 drums4easynokick]",
            "[mix 2 drums0]",
            "[mix 2 drums1]",
            "[mix 2 drums2]",
            "[mix 2 drums2d]",
            "[mix 2 drums3]",
            "[mix 2 drums3d]",
            "[mix 2 drums4]",
            "[mix 3 drums0]",
            "[mix 3 drums0d]",
            "[mix 3 drums1]",
            "[mix 3 drums1d]",
            "[mix 3 drums2]",
            "[mix 3 drums2d]",
            "[mix 3 drums3]",
            "[mix 3 drums3d]",
            "[mix 3 drums4]",
            "[mix 3 drums4d]",
            "[music_end]",
            "[music_start]",
            "[next]",
            "[play solo]",
            "[play]",
            "[play_solo]",
            "[prc_alt_chorus]",
            "[prc_alt_chorus_a]",
            "[prc_alt_chorus_b]",
            "[prc_alt_verse]",
            "[prc_alt_verse_a]",
            "[prc_band_enters]",
            "[prc_bass_riff]",
            "[prc_big_riff]",
            "[prc_big_riff_1]",
            "[prc_bre]",
            "[prc_breakdown]",
            "[prc_bridge]",
            "[prc_bridge_1]",
            "[prc_bridge_1a]",
            "[prc_bridge_1b]",
            "[prc_bridge_2]",
            "[prc_bridge_2a]",
            "[prc_bridge_2b]",
            "[prc_bridge_a]",
            "[prc_bridge_b]",
            "[prc_bridge_c]",
            "[prc_build_up]",
            "[prc_chorus_1]",
            "[prc_chorus_1a]",
            "[prc_chorus_1b]",
            "[prc_chorus_2]",
            "[prc_chorus_2a]",
            "[prc_chorus_2b]",
            "[prc_chorus_3]",
            "[prc_chorus_3a]",
            "[prc_chorus_3b]",
            "[prc_chorus_3c]",
            "[prc_chorus_3d]",
            "[prc_chorus_4]",
            "[prc_chorus_4a]",
            "[prc_chorus_4b]",
            "[prc_chorus_4c]",
            "[prc_drum_intro]",
            "[prc_ending]",
            "[prc_fast_part_1]",
            "[prc_fast_part_1a]",
            "[prc_fast_part_1b]",
            "[prc_fast_riff_1]",
            "[prc_fast_riff_2]",
            "[prc_fast_riff_2a]",
            "[prc_fast_riff_a]",
            "[prc_fast_riff_b]",
            "[prc_gtr_break]",
            "[prc_gtr_intro]",
            "[prc_gtr_intro_a]",
            "[prc_gtr_intro_b]",
            "[prc_gtr_lead]",
            "[prc_gtr_lead_a]",
            "[prc_gtr_lead_b]",
            "[prc_gtr_lead_c]",
            "[prc_gtr_line_1]",
            "[prc_gtr_line_2]",
            "[prc_gtr_melody_1]",
            "[prc_gtr_melody_2]",
            "[prc_gtr_riff]",
            "[prc_gtr_riff_1]",
            "[prc_gtr_riff_2]",
            "[prc_gtr_solo]",
            "[prc_gtr_solo_1]",
            "[prc_gtr_solo_1a]",
            "[prc_gtr_solo_2]",
            "[prc_gtr_solo_2a]",
            "[prc_gtr_solo_3]",
            "[prc_gtr_solo_3a]",
            "[prc_gtr_solo_3b]",
            "[prc_gtr_solo_4]",
            "[prc_gtr_solo_a]",
            "[prc_gtr_solo_b]",
            "[prc_gtr_solo_c]",
            "[prc_heavy_riff]",
            "[prc_heavy_riff_1]",
            "[prc_heavy_riff_2]",
            "[prc_interlude]",
            "[prc_intro]",
            "[prc_intro_a]",
            "[prc_intro_b]",
            "[prc_intro_c]",
            "[prc_intro_riff]",
            "[prc_intro_verse]",
            "[prc_main_riff]",
            "[prc_main_riff_1]",
            "[prc_main_riff_1a]",
            "[prc_main_riff_1b]",
            "[prc_main_riff_2]",
            "[prc_main_riff_3]",
            "[prc_main_riff_4]",
            "[prc_melody]",
            "[prc_odd_riff]",
            "[prc_odd_riff_1]",
            "[prc_odd_riff_2]",
            "[prc_outro]",
            "[prc_outro_a]",
            "[prc_outro_b]",
            "[prc_outro_solo]",
            "[prc_postchorus_1]",
            "[prc_postchorus_2]",
            "[prc_postchorus_a]",
            "[prc_postchorus_b]",
            "[prc_postchorus_c]",
            "[prc_prechorus_1]",
            "[prc_prechorus_1a]",
            "[prc_prechorus_2]",
            "[prc_prechorus_2a]",
            "[prc_prechorus_3]",
            "[prc_prechorus_3a]",
            "[prc_prechorus_a]",
            "[prc_prechorus_b]",
            "[prc_prechorus_c]",
            "[prc_preverse_1]",
            "[prc_preverse_2]",
            "[prc_quiet_part_a]",
            "[prc_quiet_part_b]",
            "[prc_quiet_part_c]",
            "[prc_slow_riff]",
            "[prc_slow_riff_1]",
            "[prc_slow_riff_1a]",
            "[prc_vamp]",
            "[prc_verse_1]",
            "[prc_verse_1a]",
            "[prc_verse_1b]",
            "[prc_verse_2]",
            "[prc_verse_2a]",
            "[prc_verse_2b]",
            "[prc_verse_3]",
            "[prc_verse_3a]",
            "[prc_verse_3b]",
            "[prc_verse_3c]",
            "[prc_verse_4]",
            "[prc_verse_5a]",
            "[prc_verse_5b]",
            "[prc_verse_6]",
            "[prc_verse_7]",
            "[prc_verse_riff]",
            "[prc_verse_riff_1]",
            "[prc_verse_riff_2]",
            "[prc_vocal_break]",
            "[preview]",
            "[ProFilm_a.pp]",
            "[ride_side_true]",
            "[secrion main_riff_1]",
            "[secrion main_riff_2]",
            "[secrion main_riff_3]",
            "[section aco_gtr_intro]",
            "[section band_enters]",
            "[section bass_intro]",
            "[section bass_solo]",
            "[section big_riff_1]",
            "[section big_riff_2]",
            "[section big_rock_ending]",
            "[section break]",
            "[section break_1]",
            "[section break_2]",
            "[section break_a]",
            "[section break_b]",
            "[section breakdown]",
            "[section bridge]",
            "[section bridge_1]",
            "[section bridge_1a]",
            "[section bridge_1b]",
            "[section bridge_2]",
            "[section bridge_2a]",
            "[section bridge_2b]",
            "[section bridge_2c]",
            "[section bridge_3]",
            "[section bridge_3a]",
            "[section bridge_3b]",
            "[section bridge_3c]",
            "[section bridge_a]",
            "[section bridge_b]",
            "[section bridge_c]",
            "[section bridge_d]",
            "[section buildup]",
            "[section chorus]",
            "[section chorus_1]",
            "[section chorus_1a]",
            "[section chorus_1b]",
            "[section chorus_1c]",
            "[section chorus_2]",
            "[section chorus_2a]",
            "[section chorus_2b]",
            "[section chorus_2c]",
            "[section chorus_3]",
            "[section chorus_3]",
            "[section chorus_3a]",
            "[section chorus_3b]",
            "[section chorus_3c]",
            "[section chorus_4]",
            "[section chorus_4a]",
            "[section chorus_5]",
            "[section chorus_6]",
            "[section chorus_riff]",
            "[section drum_intro]",
            "[section drum_roll_1]",
            "[section drum_roll_2]",
            "[section drum_solo]",
            "[section drums_enter]",
            "[section end]",
            "[section ending]",
            "[section fast_part]",
            "[section fast_picking]",
            "[section gtr_break]",
            "[section gtr_break_1]",
            "[section gtr_break_2]",
            "[section gtr_break_3]",
            "[section gtr_enters]",
            "[section gtr_hook]",
            "[section gtr_hook_1]",
            "[section gtr_hook_2]",
            "[section gtr_hook_3]",
            "[section gtr_intro]",
            "[section gtr_intro_a]",
            "[section gtr_intro_b]",
            "[section gtr_intro_c]",
            "[section gtr_intro_d]",
            "[section gtr_intro_e]",
            "[section gtr_lead]",
            "[section gtr_lead_1]",
            "[section gtr_lead_2]",
            "[section gtr_lead_3]",
            "[section gtr_lick]",
            "[section gtr_lick_1]",
            "[section gtr_lick_2]",
            "[section gtr_lick_3]",
            "[section gtr_lick_4]",
            "[section gtr_line]",
            "[section gtr_line_1]",
            "[section gtr_line_2]",
            "[section gtr_line_3]",
            "[section gtr_mel_1]",
            "[section gtr_mel_2]",
            "[section gtr_melody]",
            "[section gtr_riff]",
            "[section gtr_riff_1]",
            "[section gtr_riff_1a]",
            "[section gtr_riff_1b]",
            "[section gtr_riff_2]",
            "[section gtr_riff_2a]",
            "[section gtr_riff_2b]",
            "[section gtr_riff_3]",
            "[section gtr_solo]",
            "[section gtr_solo_1]",
            "[section gtr_solo_1a]",
            "[section gtr_solo_1b]",
            "[section gtr_solo_1c]",
            "[section gtr_solo_1d]",
            "[section gtr_solo_1e]",
            "[section gtr_solo_1f]",
            "[section gtr_solo_1g]",
            "[section gtr_solo_1h]",
            "[section gtr_solo_2]",
            "[section gtr_solo_2a]",
            "[section gtr_solo_2b]",
            "[section gtr_solo_2c]",
            "[section gtr_solo_2d]",
            "[section gtr_solo_2e]",
            "[section gtr_solo_2f]",
            "[section gtr_solo_3]",
            "[section gtr_solo_3a]",
            "[section gtr_solo_3b]",
            "[section gtr_solo_3c]",
            "[section gtr_solo_3d]",
            "[section gtr_solo_3e]",
            "[section gtr_solo_4]",
            "[section gtr_solo_4a]",
            "[section gtr_solo_4b]",
            "[section gtr_solo_4c]",
            "[section gtr_solo_4d]",
            "[section gtr_solo_5]",
            "[section gtr_solo_6]",
            "[section gtr_solo_7]",
            "[section gtr_solo_8]",
            "[section gtr_solo_a]",
            "[section gtr_solo_b]",
            "[section gtr_solo_c]",
            "[section gtr_solo_d]",
            "[section gtr_solo_e]",
            "[section gtr_solo_f]",
            "[section harmonica_solo]",
            "[section hook_1]",
            "[section hook_2]",
            "[section hook_3]",
            "[section hvy_part]",
            "[section intro]",
            "[section intro_a]",
            "[section intro_b]",
            "[section intro_c]",
            "[section intro_d]",
            "[section intro_fast]",
            "[section intro_heavy]",
            "[section intro_riff]",
            "[section intro_riff_a]",
            "[section intro_riff_b]",
            "[section intro_verse]",
            "[section jam]",
            "[section keyboard_solo]",
            "[section kick_it]",
            "[section loud_part]",
            "[section main_riff]",
            "[section main_riff_1]",
            "[section main_riff_2]",
            "[section main_riff_3]",
            "[section main_riff_4]",
            "[section main_riff_5]",
            "[section main_riff_6]",
            "[section melody_1]",
            "[section melody_2]",
            "[section noise_build]",
            "[section noise_intro]",
            "[section odd_riff_1]",
            "[section odd_riff_2]",
            "[section odd_riff_3]",
            "[section organ_break]",
            "[section organ_intro]",
            "[section organ_solo]",
            "[section organ_solo_a]",
            "[section organ_solo_b]",
            "[section outro]",
            "[section outro_a]",
            "[section outro_b]",
            "[section outro_c]",
            "[section outro_d]",
            "[section outro_solo]",
            "[section outro_solo_a]",
            "[section piano_intro]",
            "[section postchorus_1]",
            "[section postchorus_2]",
            "[section postchorus_3]",
            "[section postverse]",
            "[section postverse_1]",
            "[section postverse_2]",
            "[section postverse_3]",
            "[section prechorus_1]",
            "[section prechorus_2]",
            "[section prechorus_3]",
            "[section prechorus_4]",
            "[section preverse_1]",
            "[section preverse_2]",
            "[section preverse_3]",
            "[section preverse_4]",
            "[section quiet_intro]",
            "[section quiet_part]",
            "[section quiet_part_1]",
            "[section quiet_part_2]",
            "[section quiet_verse]",
            "[section sax_solo]",
            "[section slow_part]",
            "[section slow_part_a]",
            "[section slow_part_b]",
            "[section space_jam]",
            "[section spacey]",
            "[section speedup]",
            "[section speedup_a]",
            "[section speedup_b]",
            "[section swing_riff_1]",
            "[section swing_riff_2]",
            "[section swing_riff_3]",
            "[section swing_riff_4]",
            "[section synth_break]",
            "[section synth_solo]",
            "[section trippy_part]",
            "[section ugc_section_10_0]",
            "[section ugc_section_10_10]",
            "[section ugc_section_10_20]",
            "[section ugc_section_10_30]",
            "[section ugc_section_10_40]",
            "[section ugc_section_10_50]",
            "[section ugc_section_10_60]",
            "[section ugc_section_10_70]",
            "[section ugc_section_10_80]",
            "[section ugc_section_10_90]",
            "[section ugc_section_5_0]",
            "[section ugc_section_5_10]",
            "[section ugc_section_5_15]",
            "[section ugc_section_5_20]",
            "[section ugc_section_5_25]",
            "[section ugc_section_5_30]",
            "[section ugc_section_5_35]",
            "[section ugc_section_5_40]",
            "[section ugc_section_5_45]",
            "[section ugc_section_5_5]",
            "[section ugc_section_5_50]",
            "[section ugc_section_5_55]",
            "[section ugc_section_5_60]",
            "[section ugc_section_5_65]",
            "[section ugc_section_5_70]",
            "[section ugc_section_5_75]",
            "[section ugc_section_5_80]",
            "[section ugc_section_5_85]",
            "[section ugc_section_5_90]",
            "[section ugc_section_5_95]",
            "[section verse_1]",
            "[section verse_1a]",
            "[section verse_1b]",
            "[section verse_1c]",
            "[section verse_2]",
            "[section verse_2a]",
            "[section verse_2b]",
            "[section verse_2c]",
            "[section verse_3]",
            "[section verse_3a]",
            "[section verse_3b]",
            "[section verse_4]",
            "[section verse_4a]",
            "[section verse_4b]",
            "[section verse_5]",
            "[section verse_5a]",
            "[section verse_6]",
            "[section verse_6a]",
            "[section verse_6b]",
            "[section verse_7]",
            "[section verse_8]",
            "[section verse_riff]",
            "[section verse_riff_1]",
            "[section verse_riff_2]",
            "[section verse_riff_3]",
            "[section vocal_break]",
            "[section vocal_break_1]",
            "[section vocal_break_2]",
            "[section vocal_intro]",
            "[short_version]",
            "[tambourine_end]",
            "[tambourine_start]",
            "[verse]"});
            this.textBoxEventText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxEventText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxEventText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxEventText.Location = new System.Drawing.Point(9, 243);
            this.textBoxEventText.Name = "textBoxEventText";
            this.textBoxEventText.Size = new System.Drawing.Size(196, 23);
            this.textBoxEventText.TabIndex = 6;
            // 
            // buttonUpdateTextEvent
            // 
            this.buttonUpdateTextEvent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUpdateTextEvent.ImageIndex = 43;
            this.buttonUpdateTextEvent.ImageList = this.imageList1;
            this.buttonUpdateTextEvent.Location = new System.Drawing.Point(181, 268);
            this.buttonUpdateTextEvent.Name = "buttonUpdateTextEvent";
            this.buttonUpdateTextEvent.Size = new System.Drawing.Size(24, 24);
            this.buttonUpdateTextEvent.TabIndex = 9;
            this.toolTip1.SetToolTip(this.buttonUpdateTextEvent, "Update Selected");
            this.buttonUpdateTextEvent.UseVisualStyleBackColor = true;
            this.buttonUpdateTextEvent.Click += new System.EventHandler(this.buttonUpdateTextEvent_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(8, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Tick:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(8, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 15);
            this.label7.TabIndex = 5;
            this.label7.Text = "Text:";
            // 
            // groupBoxProBassTrainers
            // 
            this.groupBoxProBassTrainers.Controls.Add(this.checkTrainerLoopableProBass);
            this.groupBoxProBassTrainers.Controls.Add(this.listProBassTrainers);
            this.groupBoxProBassTrainers.Controls.Add(this.buttonRefreshProBassTrainer);
            this.groupBoxProBassTrainers.Controls.Add(this.buttonRemoveProBassTrainer);
            this.groupBoxProBassTrainers.Controls.Add(this.buttonCreateProBassTrainer);
            this.groupBoxProBassTrainers.Controls.Add(this.labelProBassTrainerStatus);
            this.groupBoxProBassTrainers.Controls.Add(this.textBoxProBassTrainerBeginTick);
            this.groupBoxProBassTrainers.Controls.Add(this.textBoxProBassTrainerEndTick);
            this.groupBoxProBassTrainers.Controls.Add(this.buttonUpdateProBassTrainer);
            this.groupBoxProBassTrainers.Controls.Add(this.buttonCancelProBassTrainer);
            this.groupBoxProBassTrainers.Controls.Add(this.label50);
            this.groupBoxProBassTrainers.Controls.Add(this.label49);
            this.groupBoxProBassTrainers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxProBassTrainers.Location = new System.Drawing.Point(200, 6);
            this.groupBoxProBassTrainers.Name = "groupBoxProBassTrainers";
            this.groupBoxProBassTrainers.Size = new System.Drawing.Size(186, 333);
            this.groupBoxProBassTrainers.TabIndex = 1;
            this.groupBoxProBassTrainers.TabStop = false;
            this.groupBoxProBassTrainers.Text = "Pro Bass Trainers";
            // 
            // checkTrainerLoopableProBass
            // 
            this.checkTrainerLoopableProBass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkTrainerLoopableProBass.Location = new System.Drawing.Point(10, 272);
            this.checkTrainerLoopableProBass.Name = "checkTrainerLoopableProBass";
            this.checkTrainerLoopableProBass.Size = new System.Drawing.Size(90, 19);
            this.checkTrainerLoopableProBass.TabIndex = 10;
            this.checkTrainerLoopableProBass.Text = "Loopable";
            this.checkTrainerLoopableProBass.UseVisualStyleBackColor = true;
            // 
            // listProBassTrainers
            // 
            this.listProBassTrainers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listProBassTrainers.FormattingEnabled = true;
            this.listProBassTrainers.ItemHeight = 15;
            this.listProBassTrainers.Location = new System.Drawing.Point(6, 19);
            this.listProBassTrainers.Name = "listProBassTrainers";
            this.listProBassTrainers.Size = new System.Drawing.Size(174, 169);
            this.listProBassTrainers.TabIndex = 0;
            this.listProBassTrainers.SelectedIndexChanged += new System.EventHandler(this.listProBassTrainers_SelectedIndexChanged);
            // 
            // buttonRefreshProBassTrainer
            // 
            this.buttonRefreshProBassTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRefreshProBassTrainer.ImageIndex = 36;
            this.buttonRefreshProBassTrainer.ImageList = this.imageList1;
            this.buttonRefreshProBassTrainer.Location = new System.Drawing.Point(156, 194);
            this.buttonRefreshProBassTrainer.Name = "buttonRefreshProBassTrainer";
            this.buttonRefreshProBassTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonRefreshProBassTrainer.TabIndex = 1;
            this.toolTip1.SetToolTip(this.buttonRefreshProBassTrainer, "Refresh - Also Unselects");
            this.buttonRefreshProBassTrainer.UseVisualStyleBackColor = true;
            this.buttonRefreshProBassTrainer.Click += new System.EventHandler(this.buttonRefreshProBassTrainer_Click);
            // 
            // buttonRemoveProBassTrainer
            // 
            this.buttonRemoveProBassTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRemoveProBassTrainer.ImageIndex = 32;
            this.buttonRemoveProBassTrainer.ImageList = this.imageList1;
            this.buttonRemoveProBassTrainer.Location = new System.Drawing.Point(7, 194);
            this.buttonRemoveProBassTrainer.Name = "buttonRemoveProBassTrainer";
            this.buttonRemoveProBassTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonRemoveProBassTrainer.TabIndex = 4;
            this.buttonRemoveProBassTrainer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveProBassTrainer, "Delete Selected Trainer");
            this.buttonRemoveProBassTrainer.UseVisualStyleBackColor = true;
            this.buttonRemoveProBassTrainer.Click += new System.EventHandler(this.buttonRemoveProBassTrainer_Click);
            // 
            // buttonCreateProBassTrainer
            // 
            this.buttonCreateProBassTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCreateProBassTrainer.ImageIndex = 35;
            this.buttonCreateProBassTrainer.ImageList = this.imageList1;
            this.buttonCreateProBassTrainer.Location = new System.Drawing.Point(126, 194);
            this.buttonCreateProBassTrainer.Name = "buttonCreateProBassTrainer";
            this.buttonCreateProBassTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonCreateProBassTrainer.TabIndex = 2;
            this.toolTip1.SetToolTip(this.buttonCreateProBassTrainer, "Create New Pro Bass Trainer (B)");
            this.buttonCreateProBassTrainer.UseVisualStyleBackColor = true;
            this.buttonCreateProBassTrainer.Click += new System.EventHandler(this.buttonCreateProBassTrainer_Click);
            // 
            // labelProBassTrainerStatus
            // 
            this.labelProBassTrainerStatus.AutoSize = true;
            this.labelProBassTrainerStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProBassTrainerStatus.Location = new System.Drawing.Point(11, 294);
            this.labelProBassTrainerStatus.Name = "labelProBassTrainerStatus";
            this.labelProBassTrainerStatus.Size = new System.Drawing.Size(26, 15);
            this.labelProBassTrainerStatus.TabIndex = 11;
            this.labelProBassTrainerStatus.Text = "Idle";
            // 
            // textBoxProBassTrainerBeginTick
            // 
            this.textBoxProBassTrainerBeginTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxProBassTrainerBeginTick.Location = new System.Drawing.Point(10, 243);
            this.textBoxProBassTrainerBeginTick.Name = "textBoxProBassTrainerBeginTick";
            this.textBoxProBassTrainerBeginTick.Size = new System.Drawing.Size(80, 23);
            this.textBoxProBassTrainerBeginTick.TabIndex = 6;
            // 
            // textBoxProBassTrainerEndTick
            // 
            this.textBoxProBassTrainerEndTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxProBassTrainerEndTick.Location = new System.Drawing.Point(96, 243);
            this.textBoxProBassTrainerEndTick.Name = "textBoxProBassTrainerEndTick";
            this.textBoxProBassTrainerEndTick.Size = new System.Drawing.Size(85, 23);
            this.textBoxProBassTrainerEndTick.TabIndex = 8;
            // 
            // buttonUpdateProBassTrainer
            // 
            this.buttonUpdateProBassTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUpdateProBassTrainer.ImageIndex = 43;
            this.buttonUpdateProBassTrainer.ImageList = this.imageList1;
            this.buttonUpdateProBassTrainer.Location = new System.Drawing.Point(156, 268);
            this.buttonUpdateProBassTrainer.Name = "buttonUpdateProBassTrainer";
            this.buttonUpdateProBassTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonUpdateProBassTrainer.TabIndex = 9;
            this.toolTip1.SetToolTip(this.buttonUpdateProBassTrainer, "Update Selected");
            this.buttonUpdateProBassTrainer.UseVisualStyleBackColor = true;
            this.buttonUpdateProBassTrainer.Click += new System.EventHandler(this.buttonUpdateProBassTrainer_Click);
            // 
            // buttonCancelProBassTrainer
            // 
            this.buttonCancelProBassTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCancelProBassTrainer.ImageIndex = 31;
            this.buttonCancelProBassTrainer.ImageList = this.imageList1;
            this.buttonCancelProBassTrainer.Location = new System.Drawing.Point(96, 194);
            this.buttonCancelProBassTrainer.Name = "buttonCancelProBassTrainer";
            this.buttonCancelProBassTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonCancelProBassTrainer.TabIndex = 3;
            this.toolTip1.SetToolTip(this.buttonCancelProBassTrainer, "Cancel Create New Trainer");
            this.buttonCancelProBassTrainer.UseVisualStyleBackColor = true;
            this.buttonCancelProBassTrainer.Click += new System.EventHandler(this.buttonCancelProBassTrainer_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label50.Location = new System.Drawing.Point(93, 225);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(24, 15);
            this.label50.TabIndex = 7;
            this.label50.Text = "To:";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label49.Location = new System.Drawing.Point(11, 225);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(38, 15);
            this.label49.TabIndex = 5;
            this.label49.Text = "From:";
            // 
            // groupBoxProGuitarTrainers
            // 
            this.groupBoxProGuitarTrainers.Controls.Add(this.checkTrainerLoopableProGuitar);
            this.groupBoxProGuitarTrainers.Controls.Add(this.listProGuitarTrainers);
            this.groupBoxProGuitarTrainers.Controls.Add(this.button135);
            this.groupBoxProGuitarTrainers.Controls.Add(this.buttonRemoveProGuitarTrainer);
            this.groupBoxProGuitarTrainers.Controls.Add(this.buttonAddProGuitarTrainer);
            this.groupBoxProGuitarTrainers.Controls.Add(this.labelProGuitarTrainerStatus);
            this.groupBoxProGuitarTrainers.Controls.Add(this.textBoxProGuitarTrainerBeginTick);
            this.groupBoxProGuitarTrainers.Controls.Add(this.textBoxProGuitarTrainerEndTick);
            this.groupBoxProGuitarTrainers.Controls.Add(this.buttonUpdateProGuitarTrainer);
            this.groupBoxProGuitarTrainers.Controls.Add(this.buttonCancelProGuitarTrainer);
            this.groupBoxProGuitarTrainers.Controls.Add(this.label51);
            this.groupBoxProGuitarTrainers.Controls.Add(this.label52);
            this.groupBoxProGuitarTrainers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBoxProGuitarTrainers.Location = new System.Drawing.Point(8, 6);
            this.groupBoxProGuitarTrainers.Name = "groupBoxProGuitarTrainers";
            this.groupBoxProGuitarTrainers.Size = new System.Drawing.Size(186, 333);
            this.groupBoxProGuitarTrainers.TabIndex = 0;
            this.groupBoxProGuitarTrainers.TabStop = false;
            this.groupBoxProGuitarTrainers.Text = "Pro Guitar Trainers";
            // 
            // checkTrainerLoopableProGuitar
            // 
            this.checkTrainerLoopableProGuitar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkTrainerLoopableProGuitar.Location = new System.Drawing.Point(7, 272);
            this.checkTrainerLoopableProGuitar.Name = "checkTrainerLoopableProGuitar";
            this.checkTrainerLoopableProGuitar.Size = new System.Drawing.Size(90, 19);
            this.checkTrainerLoopableProGuitar.TabIndex = 10;
            this.checkTrainerLoopableProGuitar.Text = "Loopable";
            this.checkTrainerLoopableProGuitar.UseVisualStyleBackColor = true;
            // 
            // listProGuitarTrainers
            // 
            this.listProGuitarTrainers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listProGuitarTrainers.FormattingEnabled = true;
            this.listProGuitarTrainers.ItemHeight = 15;
            this.listProGuitarTrainers.Location = new System.Drawing.Point(6, 19);
            this.listProGuitarTrainers.Name = "listProGuitarTrainers";
            this.listProGuitarTrainers.Size = new System.Drawing.Size(174, 169);
            this.listProGuitarTrainers.TabIndex = 0;
            this.listProGuitarTrainers.SelectedIndexChanged += new System.EventHandler(this.listProGuitarTrainers_SelectedIndexChanged);
            // 
            // button135
            // 
            this.button135.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button135.ImageIndex = 36;
            this.button135.ImageList = this.imageList1;
            this.button135.Location = new System.Drawing.Point(156, 194);
            this.button135.Name = "button135";
            this.button135.Size = new System.Drawing.Size(24, 24);
            this.button135.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button135, "Refresh - Also Unselects");
            this.button135.UseVisualStyleBackColor = true;
            this.button135.Click += new System.EventHandler(this.button135_Click);
            // 
            // buttonRemoveProGuitarTrainer
            // 
            this.buttonRemoveProGuitarTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonRemoveProGuitarTrainer.ImageIndex = 32;
            this.buttonRemoveProGuitarTrainer.ImageList = this.imageList1;
            this.buttonRemoveProGuitarTrainer.Location = new System.Drawing.Point(7, 194);
            this.buttonRemoveProGuitarTrainer.Name = "buttonRemoveProGuitarTrainer";
            this.buttonRemoveProGuitarTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonRemoveProGuitarTrainer.TabIndex = 4;
            this.buttonRemoveProGuitarTrainer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveProGuitarTrainer, "Delete Selected Trainer");
            this.buttonRemoveProGuitarTrainer.UseVisualStyleBackColor = true;
            this.buttonRemoveProGuitarTrainer.Click += new System.EventHandler(this.buttonRemoveProGuitarTrainer_Click);
            // 
            // buttonAddProGuitarTrainer
            // 
            this.buttonAddProGuitarTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonAddProGuitarTrainer.ImageIndex = 35;
            this.buttonAddProGuitarTrainer.ImageList = this.imageList1;
            this.buttonAddProGuitarTrainer.Location = new System.Drawing.Point(126, 194);
            this.buttonAddProGuitarTrainer.Name = "buttonAddProGuitarTrainer";
            this.buttonAddProGuitarTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonAddProGuitarTrainer.TabIndex = 2;
            this.toolTip1.SetToolTip(this.buttonAddProGuitarTrainer, "Create New Pro Guitar Trainer (P)");
            this.buttonAddProGuitarTrainer.UseVisualStyleBackColor = true;
            this.buttonAddProGuitarTrainer.Click += new System.EventHandler(this.buttonAddProGuitarTrainer_Click);
            // 
            // labelProGuitarTrainerStatus
            // 
            this.labelProGuitarTrainerStatus.AutoSize = true;
            this.labelProGuitarTrainerStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelProGuitarTrainerStatus.Location = new System.Drawing.Point(9, 294);
            this.labelProGuitarTrainerStatus.Name = "labelProGuitarTrainerStatus";
            this.labelProGuitarTrainerStatus.Size = new System.Drawing.Size(26, 15);
            this.labelProGuitarTrainerStatus.TabIndex = 11;
            this.labelProGuitarTrainerStatus.Text = "Idle";
            // 
            // textBoxProGuitarTrainerBeginTick
            // 
            this.textBoxProGuitarTrainerBeginTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxProGuitarTrainerBeginTick.Location = new System.Drawing.Point(7, 243);
            this.textBoxProGuitarTrainerBeginTick.Name = "textBoxProGuitarTrainerBeginTick";
            this.textBoxProGuitarTrainerBeginTick.Size = new System.Drawing.Size(83, 23);
            this.textBoxProGuitarTrainerBeginTick.TabIndex = 6;
            // 
            // textBoxProGuitarTrainerEndTick
            // 
            this.textBoxProGuitarTrainerEndTick.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxProGuitarTrainerEndTick.Location = new System.Drawing.Point(96, 243);
            this.textBoxProGuitarTrainerEndTick.Name = "textBoxProGuitarTrainerEndTick";
            this.textBoxProGuitarTrainerEndTick.Size = new System.Drawing.Size(85, 23);
            this.textBoxProGuitarTrainerEndTick.TabIndex = 8;
            // 
            // buttonUpdateProGuitarTrainer
            // 
            this.buttonUpdateProGuitarTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUpdateProGuitarTrainer.ImageIndex = 43;
            this.buttonUpdateProGuitarTrainer.ImageList = this.imageList1;
            this.buttonUpdateProGuitarTrainer.Location = new System.Drawing.Point(156, 268);
            this.buttonUpdateProGuitarTrainer.Name = "buttonUpdateProGuitarTrainer";
            this.buttonUpdateProGuitarTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonUpdateProGuitarTrainer.TabIndex = 9;
            this.toolTip1.SetToolTip(this.buttonUpdateProGuitarTrainer, "Update Selected");
            this.buttonUpdateProGuitarTrainer.UseVisualStyleBackColor = true;
            this.buttonUpdateProGuitarTrainer.Click += new System.EventHandler(this.buttonUpdateProGuitarTrainer_Click);
            // 
            // buttonCancelProGuitarTrainer
            // 
            this.buttonCancelProGuitarTrainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCancelProGuitarTrainer.ImageIndex = 31;
            this.buttonCancelProGuitarTrainer.ImageList = this.imageList1;
            this.buttonCancelProGuitarTrainer.Location = new System.Drawing.Point(96, 194);
            this.buttonCancelProGuitarTrainer.Name = "buttonCancelProGuitarTrainer";
            this.buttonCancelProGuitarTrainer.Size = new System.Drawing.Size(24, 24);
            this.buttonCancelProGuitarTrainer.TabIndex = 3;
            this.toolTip1.SetToolTip(this.buttonCancelProGuitarTrainer, "Cancel Create New Trainer");
            this.buttonCancelProGuitarTrainer.UseVisualStyleBackColor = true;
            this.buttonCancelProGuitarTrainer.Click += new System.EventHandler(this.buttonCancelProGuitarTrainer_Click);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label51.Location = new System.Drawing.Point(93, 225);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(24, 15);
            this.label51.TabIndex = 7;
            this.label51.Text = "To:";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label52.Location = new System.Drawing.Point(9, 225);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(38, 15);
            this.label52.TabIndex = 5;
            this.label52.Text = "From:";
            // 
            // tabPackageEditor
            // 
            this.tabPackageEditor.AllowDrop = true;
            this.tabPackageEditor.AutoScroll = true;
            this.tabPackageEditor.AutoScrollMinSize = new System.Drawing.Size(1010, 408);
            this.tabPackageEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tabPackageEditor.Controls.Add(this.tabControl3);
            this.tabPackageEditor.ImageIndex = 52;
            this.tabPackageEditor.Location = new System.Drawing.Point(4, 23);
            this.tabPackageEditor.Name = "tabPackageEditor";
            this.tabPackageEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPackageEditor.Size = new System.Drawing.Size(1095, 523);
            this.tabPackageEditor.TabIndex = 7;
            this.tabPackageEditor.Text = "Package Editor";
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1089, 517);
            this.tabControl3.TabIndex = 23;
            // 
            // tabPage8
            // 
            this.tabPage8.AllowDrop = true;
            this.tabPage8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabPage8.Controls.Add(this.groupBox32);
            this.tabPage8.Location = new System.Drawing.Point(4, 24);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1081, 489);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "Package Viewer";
            this.tabPage8.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabPackageViewer_FileDrop);
            this.tabPage8.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabPage8_DragEnter);
            this.tabPage8.DragOver += new System.Windows.Forms.DragEventHandler(this.tabPackageViewer_DragOver);
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.buttonPackageViewerSave);
            this.groupBox32.Controls.Add(this.checkBoxPackageEditExtractDTAMidOnly);
            this.groupBox32.Controls.Add(this.button9);
            this.groupBox32.Controls.Add(this.button67);
            this.groupBox32.Controls.Add(this.textBoxPackageDTAText);
            this.groupBox32.Controls.Add(this.treePackageContents);
            this.groupBox32.Controls.Add(this.buttonPackageEditorOpenPackage);
            this.groupBox32.Controls.Add(this.label41);
            this.groupBox32.Controls.Add(this.button60);
            this.groupBox32.Controls.Add(this.label42);
            this.groupBox32.Controls.Add(this.button81);
            this.groupBox32.Controls.Add(this.checkBox2);
            this.groupBox32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox32.Location = new System.Drawing.Point(3, 3);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Size = new System.Drawing.Size(1075, 485);
            this.groupBox32.TabIndex = 0;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "Package Viewer";
            // 
            // buttonPackageViewerSave
            // 
            this.buttonPackageViewerSave.ForeColor = System.Drawing.Color.Black;
            this.buttonPackageViewerSave.ImageIndex = 30;
            this.buttonPackageViewerSave.ImageList = this.imageList1;
            this.buttonPackageViewerSave.Location = new System.Drawing.Point(9, 302);
            this.buttonPackageViewerSave.Name = "buttonPackageViewerSave";
            this.buttonPackageViewerSave.Size = new System.Drawing.Size(69, 24);
            this.buttonPackageViewerSave.TabIndex = 39;
            this.buttonPackageViewerSave.Text = "Save";
            this.buttonPackageViewerSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPackageViewerSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPackageViewerSave.UseVisualStyleBackColor = true;
            this.buttonPackageViewerSave.Click += new System.EventHandler(this.buttonPackageViewerSave_Click);
            // 
            // checkBoxPackageEditExtractDTAMidOnly
            // 
            this.checkBoxPackageEditExtractDTAMidOnly.Checked = true;
            this.checkBoxPackageEditExtractDTAMidOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPackageEditExtractDTAMidOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxPackageEditExtractDTAMidOnly.Location = new System.Drawing.Point(250, 356);
            this.checkBoxPackageEditExtractDTAMidOnly.Name = "checkBoxPackageEditExtractDTAMidOnly";
            this.checkBoxPackageEditExtractDTAMidOnly.Size = new System.Drawing.Size(144, 20);
            this.checkBoxPackageEditExtractDTAMidOnly.TabIndex = 38;
            this.checkBoxPackageEditExtractDTAMidOnly.Text = "DTA and MID Only";
            this.checkBoxPackageEditExtractDTAMidOnly.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button9.ImageIndex = 10;
            this.button9.ImageList = this.imageList1;
            this.button9.Location = new System.Drawing.Point(106, 19);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(91, 24);
            this.button9.TabIndex = 37;
            this.button9.Text = "Open Song";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button67
            // 
            this.button67.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button67.ImageIndex = 16;
            this.button67.ImageList = this.imageList1;
            this.button67.Location = new System.Drawing.Point(199, 302);
            this.button67.Name = "button67";
            this.button67.Size = new System.Drawing.Size(24, 24);
            this.button67.TabIndex = 36;
            this.toolTip1.SetToolTip(this.button67, "Check Package for Errors");
            this.button67.UseVisualStyleBackColor = true;
            this.button67.Click += new System.EventHandler(this.button67_Click_1);
            // 
            // textBoxPackageDTAText
            // 
            this.textBoxPackageDTAText.AcceptsReturn = true;
            this.textBoxPackageDTAText.AcceptsTab = true;
            this.textBoxPackageDTAText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPackageDTAText.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPackageDTAText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPackageDTAText.Font = new System.Drawing.Font("Courier New", 10F);
            this.textBoxPackageDTAText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxPackageDTAText.Location = new System.Drawing.Point(448, 32);
            this.textBoxPackageDTAText.MaxLength = 327670;
            this.textBoxPackageDTAText.Multiline = true;
            this.textBoxPackageDTAText.Name = "textBoxPackageDTAText";
            this.textBoxPackageDTAText.ReadOnly = true;
            this.textBoxPackageDTAText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPackageDTAText.Size = new System.Drawing.Size(614, 351);
            this.textBoxPackageDTAText.TabIndex = 5;
            this.textBoxPackageDTAText.WordWrap = false;
            // 
            // treePackageContents
            // 
            this.treePackageContents.AllowDrop = true;
            this.treePackageContents.ContextMenuStrip = this.contextToolStripPackageEditor;
            this.treePackageContents.ForeColor = System.Drawing.SystemColors.ControlText;
            this.treePackageContents.HideSelection = false;
            this.treePackageContents.ImageKey = "XPFolder.gif";
            this.treePackageContents.ImageList = this.imageList1;
            this.treePackageContents.Location = new System.Drawing.Point(9, 62);
            this.treePackageContents.Name = "treePackageContents";
            this.treePackageContents.SelectedImageKey = "XPFolder.gif";
            this.treePackageContents.Size = new System.Drawing.Size(433, 234);
            this.treePackageContents.TabIndex = 1;
            this.treePackageContents.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePackageContents_AfterSelect);
            this.treePackageContents.DragDrop += new System.Windows.Forms.DragEventHandler(this.treePackageContents_DragDrop);
            this.treePackageContents.DragEnter += new System.Windows.Forms.DragEventHandler(this.treePackageContents_DragEnter);
            this.treePackageContents.DragOver += new System.Windows.Forms.DragEventHandler(this.treePackageContents_DragOver);
            this.treePackageContents.DoubleClick += new System.EventHandler(this.treePackageContents_DoubleClick);
            // 
            // contextToolStripPackageEditor
            // 
            this.contextToolStripPackageEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPackageEditorDeleteFile});
            this.contextToolStripPackageEditor.Name = "contextToolStripPackageEditor";
            this.contextToolStripPackageEditor.Size = new System.Drawing.Size(129, 26);
            this.contextToolStripPackageEditor.Opening += new System.ComponentModel.CancelEventHandler(this.contextToolStripPackageEditor_Opening);
            // 
            // toolStripPackageEditorDeleteFile
            // 
            this.toolStripPackageEditorDeleteFile.Name = "toolStripPackageEditorDeleteFile";
            this.toolStripPackageEditorDeleteFile.Size = new System.Drawing.Size(128, 22);
            this.toolStripPackageEditorDeleteFile.Text = "Delete File";
            this.toolStripPackageEditorDeleteFile.Click += new System.EventHandler(this.toolStripPackageEditorDeleteFile_Click);
            // 
            // buttonPackageEditorOpenPackage
            // 
            this.buttonPackageEditorOpenPackage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonPackageEditorOpenPackage.ImageIndex = 29;
            this.buttonPackageEditorOpenPackage.ImageList = this.imageList1;
            this.buttonPackageEditorOpenPackage.Location = new System.Drawing.Point(9, 19);
            this.buttonPackageEditorOpenPackage.Name = "buttonPackageEditorOpenPackage";
            this.buttonPackageEditorOpenPackage.Size = new System.Drawing.Size(91, 24);
            this.buttonPackageEditorOpenPackage.TabIndex = 0;
            this.buttonPackageEditorOpenPackage.Text = "Open File";
            this.buttonPackageEditorOpenPackage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPackageEditorOpenPackage.UseVisualStyleBackColor = true;
            this.buttonPackageEditorOpenPackage.Click += new System.EventHandler(this.buttonPackageEditorOpenPackage_Click);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.label41.Location = new System.Drawing.Point(6, 46);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(58, 15);
            this.label41.TabIndex = 6;
            this.label41.Text = "Contents:";
            // 
            // button60
            // 
            this.button60.BackColor = System.Drawing.Color.Transparent;
            this.button60.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button60.ImageIndex = 48;
            this.button60.ImageList = this.imageList1;
            this.button60.Location = new System.Drawing.Point(229, 302);
            this.button60.Name = "button60";
            this.button60.Size = new System.Drawing.Size(87, 24);
            this.button60.TabIndex = 2;
            this.button60.Text = "Extract All";
            this.button60.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button60, "Extract All");
            this.button60.UseVisualStyleBackColor = true;
            this.button60.Click += new System.EventHandler(this.button60_Click_1);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.label42.Location = new System.Drawing.Point(445, 16);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(128, 15);
            this.label42.TabIndex = 11;
            this.label42.Text = "Selected File Properties";
            // 
            // button81
            // 
            this.button81.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button81.ImageIndex = 47;
            this.button81.ImageList = this.imageList1;
            this.button81.Location = new System.Drawing.Point(322, 302);
            this.button81.Name = "button81";
            this.button81.Size = new System.Drawing.Size(120, 24);
            this.button81.TabIndex = 3;
            this.button81.Text = "Extract Selected";
            this.button81.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button81.UseVisualStyleBackColor = true;
            this.button81.Click += new System.EventHandler(this.button81_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox2.Location = new System.Drawing.Point(250, 332);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(144, 19);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Open In New Window";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabPage6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage6.Controls.Add(this.groupBox31);
            this.tabPage6.Controls.Add(this.groupBox30);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1081, 491);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Package Utility";
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.checkBox11);
            this.groupBox31.Controls.Add(this.textBox49);
            this.groupBox31.Controls.Add(this.label4);
            this.groupBox31.Controls.Add(this.button91);
            this.groupBox31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox31.Location = new System.Drawing.Point(6, 6);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(302, 79);
            this.groupBox31.TabIndex = 0;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Extract Package";
            // 
            // checkBox11
            // 
            this.checkBox11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBox11.Location = new System.Drawing.Point(51, 53);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(168, 17);
            this.checkBox11.TabIndex = 19;
            this.checkBox11.Text = "Only Songs.dta and .mid";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // textBox49
            // 
            this.textBox49.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox49.Location = new System.Drawing.Point(51, 23);
            this.textBox49.Name = "textBox49";
            this.textBox49.Size = new System.Drawing.Size(245, 23);
            this.textBox49.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(13, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Path:";
            // 
            // button91
            // 
            this.button91.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button91.Location = new System.Drawing.Point(229, 49);
            this.button91.Name = "button91";
            this.button91.Size = new System.Drawing.Size(67, 23);
            this.button91.TabIndex = 1;
            this.button91.Text = "Extract";
            this.button91.UseVisualStyleBackColor = true;
            this.button91.Click += new System.EventHandler(this.button91_Click);
            // 
            // groupBox30
            // 
            this.groupBox30.Controls.Add(this.buttonCheckAllInFolder);
            this.groupBox30.Controls.Add(this.button92);
            this.groupBox30.Controls.Add(this.textBox25);
            this.groupBox30.Controls.Add(this.label1);
            this.groupBox30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox30.Location = new System.Drawing.Point(4, 91);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(304, 79);
            this.groupBox30.TabIndex = 1;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "Extract All Packages in Folder";
            // 
            // buttonCheckAllInFolder
            // 
            this.buttonCheckAllInFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCheckAllInFolder.ImageIndex = 16;
            this.buttonCheckAllInFolder.ImageList = this.imageList1;
            this.buttonCheckAllInFolder.Location = new System.Drawing.Point(197, 48);
            this.buttonCheckAllInFolder.Name = "buttonCheckAllInFolder";
            this.buttonCheckAllInFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonCheckAllInFolder.TabIndex = 37;
            this.toolTip1.SetToolTip(this.buttonCheckAllInFolder, "Check Package for Errors");
            this.buttonCheckAllInFolder.UseVisualStyleBackColor = true;
            this.buttonCheckAllInFolder.Click += new System.EventHandler(this.buttonCheckAllInFolder_Click);
            // 
            // button92
            // 
            this.button92.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button92.Location = new System.Drawing.Point(227, 49);
            this.button92.Name = "button92";
            this.button92.Size = new System.Drawing.Size(71, 23);
            this.button92.TabIndex = 1;
            this.button92.Text = "Extract";
            this.button92.UseVisualStyleBackColor = true;
            this.button92.Click += new System.EventHandler(this.button92_Click);
            // 
            // textBox25
            // 
            this.textBox25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox25.Location = new System.Drawing.Point(51, 23);
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new System.Drawing.Size(247, 23);
            this.textBox25.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Folder:";
            // 
            // tabUSBDrive
            // 
            this.tabUSBDrive.AutoScroll = true;
            this.tabUSBDrive.AutoScrollMinSize = new System.Drawing.Size(1010, 408);
            this.tabUSBDrive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabUSBDrive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabUSBDrive.Controls.Add(this.buttonUSBCheckFile);
            this.tabUSBDrive.Controls.Add(this.listBoxUSBSongs);
            this.tabUSBDrive.Controls.Add(this.listUSBFileView);
            this.tabUSBDrive.Controls.Add(this.treeUSBContents);
            this.tabUSBDrive.Controls.Add(this.progressUSBFiles);
            this.tabUSBDrive.Controls.Add(this.textBoxUSBFileName);
            this.tabUSBDrive.Controls.Add(this.buttonUSBAddFolder);
            this.tabUSBDrive.Controls.Add(this.progressUSBSongs);
            this.tabUSBDrive.Controls.Add(this.label47);
            this.tabUSBDrive.Controls.Add(this.buttonUSBRestoreImage);
            this.tabUSBDrive.Controls.Add(this.buttonUSBCreateImage);
            this.tabUSBDrive.Controls.Add(this.textBoxUSBFolder);
            this.tabUSBDrive.Controls.Add(this.label46);
            this.tabUSBDrive.Controls.Add(this.comboUSBList);
            this.tabUSBDrive.Controls.Add(this.label45);
            this.tabUSBDrive.Controls.Add(this.buttonUSBRenameFile);
            this.tabUSBDrive.Controls.Add(this.buttonUSBViewPackage);
            this.tabUSBDrive.Controls.Add(this.buttonUSBSelectCompletedSongs);
            this.tabUSBDrive.Controls.Add(this.buttonUSBSelectAllSongs);
            this.tabUSBDrive.Controls.Add(this.buttonUSBAddFile);
            this.tabUSBDrive.Controls.Add(this.buttonUSBDeleteFile);
            this.tabUSBDrive.Controls.Add(this.buttonUSBSetDefaultFolder);
            this.tabUSBDrive.Controls.Add(this.buttonUSBRenameFolder);
            this.tabUSBDrive.Controls.Add(this.buttonUSBCreateFolder);
            this.tabUSBDrive.Controls.Add(this.buttonUSBRefresh);
            this.tabUSBDrive.Controls.Add(this.buttonUSBCopySelectedSongToUSB);
            this.tabUSBDrive.Controls.Add(this.buttonUSBExtractSelectedFiles);
            this.tabUSBDrive.Controls.Add(this.buttonUSBExtractFolder);
            this.tabUSBDrive.Controls.Add(this.buttonUSBDeleteSelected);
            this.tabUSBDrive.Controls.Add(this.button125);
            this.tabUSBDrive.ImageIndex = 22;
            this.tabUSBDrive.Location = new System.Drawing.Point(4, 23);
            this.tabUSBDrive.Name = "tabUSBDrive";
            this.tabUSBDrive.Padding = new System.Windows.Forms.Padding(3);
            this.tabUSBDrive.Size = new System.Drawing.Size(1095, 523);
            this.tabUSBDrive.TabIndex = 9;
            this.tabUSBDrive.Text = "USB Drive";
            // 
            // buttonUSBCheckFile
            // 
            this.buttonUSBCheckFile.ImageIndex = 16;
            this.buttonUSBCheckFile.ImageList = this.imageList1;
            this.buttonUSBCheckFile.Location = new System.Drawing.Point(614, 292);
            this.buttonUSBCheckFile.Name = "buttonUSBCheckFile";
            this.buttonUSBCheckFile.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBCheckFile.TabIndex = 39;
            this.toolTip1.SetToolTip(this.buttonUSBCheckFile, "Check Package");
            this.buttonUSBCheckFile.UseVisualStyleBackColor = true;
            this.buttonUSBCheckFile.Click += new System.EventHandler(this.buttonUSBCheckFile_Click);
            // 
            // listBoxUSBSongs
            // 
            this.listBoxUSBSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxUSBSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listBoxUSBSongs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listBoxUSBSongs.HideSelection = false;
            this.listBoxUSBSongs.Location = new System.Drawing.Point(760, 52);
            this.listBoxUSBSongs.Name = "listBoxUSBSongs";
            this.listBoxUSBSongs.Size = new System.Drawing.Size(329, 225);
            this.listBoxUSBSongs.TabIndex = 15;
            this.listBoxUSBSongs.UseCompatibleStateImageBehavior = false;
            this.listBoxUSBSongs.View = System.Windows.Forms.View.Details;
            this.listBoxUSBSongs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listBoxUSBSongs_ItemDrag);
            // 
            // listUSBFileView
            // 
            this.listUSBFileView.AllowColumnReorder = true;
            this.listUSBFileView.AllowDrop = true;
            this.listUSBFileView.GridLines = true;
            listViewGroup4.Header = "File Name";
            listViewGroup4.Name = "FileName";
            listViewGroup5.Header = "Size";
            listViewGroup5.Name = "Size";
            listViewGroup6.Header = "Date";
            listViewGroup6.Name = "Date";
            this.listUSBFileView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.listUSBFileView.HideSelection = false;
            this.listUSBFileView.LabelEdit = true;
            this.listUSBFileView.LabelWrap = false;
            this.listUSBFileView.Location = new System.Drawing.Point(315, 52);
            this.listUSBFileView.Name = "listUSBFileView";
            this.listUSBFileView.Size = new System.Drawing.Size(439, 225);
            this.listUSBFileView.TabIndex = 32;
            this.listUSBFileView.UseCompatibleStateImageBehavior = false;
            this.listUSBFileView.View = System.Windows.Forms.View.List;
            this.listUSBFileView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listUSBFileView_AfterLabelEdit);
            this.listUSBFileView.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listUSBFileView_BeforeLabelEdit);
            this.listUSBFileView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listUSBFileView_ItemDrag);
            this.listUSBFileView.SelectedIndexChanged += new System.EventHandler(this.listUSBFileView_SelectedIndexChanged_1);
            this.listUSBFileView.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDropListUSBFileView);
            this.listUSBFileView.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnterListUSBFileView);
            this.listUSBFileView.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOverListUSBFileView);
            this.listUSBFileView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listUSBFileView_MouseDoubleClick);
            // 
            // treeUSBContents
            // 
            this.treeUSBContents.HideSelection = false;
            this.treeUSBContents.Location = new System.Drawing.Point(8, 52);
            this.treeUSBContents.Name = "treeUSBContents";
            this.treeUSBContents.ShowPlusMinus = false;
            this.treeUSBContents.ShowRootLines = false;
            this.treeUSBContents.Size = new System.Drawing.Size(301, 225);
            this.treeUSBContents.TabIndex = 8;
            // 
            // progressUSBFiles
            // 
            this.progressUSBFiles.Location = new System.Drawing.Point(315, 280);
            this.progressUSBFiles.Name = "progressUSBFiles";
            this.progressUSBFiles.Size = new System.Drawing.Size(439, 11);
            this.progressUSBFiles.TabIndex = 38;
            // 
            // textBoxUSBFileName
            // 
            this.textBoxUSBFileName.Location = new System.Drawing.Point(315, 296);
            this.textBoxUSBFileName.Name = "textBoxUSBFileName";
            this.textBoxUSBFileName.Size = new System.Drawing.Size(215, 23);
            this.textBoxUSBFileName.TabIndex = 36;
            // 
            // buttonUSBAddFolder
            // 
            this.buttonUSBAddFolder.Location = new System.Drawing.Point(128, 382);
            this.buttonUSBAddFolder.Name = "buttonUSBAddFolder";
            this.buttonUSBAddFolder.Size = new System.Drawing.Size(140, 23);
            this.buttonUSBAddFolder.TabIndex = 34;
            this.buttonUSBAddFolder.Text = "Add Folder";
            this.buttonUSBAddFolder.UseVisualStyleBackColor = true;
            this.buttonUSBAddFolder.Visible = false;
            this.buttonUSBAddFolder.Click += new System.EventHandler(this.buttonUSBAddFolder_Click_1);
            // 
            // progressUSBSongs
            // 
            this.progressUSBSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressUSBSongs.Location = new System.Drawing.Point(760, 280);
            this.progressUSBSongs.Name = "progressUSBSongs";
            this.progressUSBSongs.Size = new System.Drawing.Size(329, 11);
            this.progressUSBSongs.TabIndex = 33;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.label47.Location = new System.Drawing.Point(312, 36);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(33, 15);
            this.label47.TabIndex = 27;
            this.label47.Text = "Files:";
            // 
            // buttonUSBRestoreImage
            // 
            this.buttonUSBRestoreImage.Location = new System.Drawing.Point(8, 382);
            this.buttonUSBRestoreImage.Name = "buttonUSBRestoreImage";
            this.buttonUSBRestoreImage.Size = new System.Drawing.Size(104, 22);
            this.buttonUSBRestoreImage.TabIndex = 25;
            this.buttonUSBRestoreImage.Text = "Restore Image";
            this.buttonUSBRestoreImage.UseVisualStyleBackColor = true;
            this.buttonUSBRestoreImage.Visible = false;
            // 
            // buttonUSBCreateImage
            // 
            this.buttonUSBCreateImage.Location = new System.Drawing.Point(8, 354);
            this.buttonUSBCreateImage.Name = "buttonUSBCreateImage";
            this.buttonUSBCreateImage.Size = new System.Drawing.Size(104, 22);
            this.buttonUSBCreateImage.TabIndex = 24;
            this.buttonUSBCreateImage.Text = "Create Image";
            this.buttonUSBCreateImage.UseVisualStyleBackColor = true;
            this.buttonUSBCreateImage.Visible = false;
            // 
            // textBoxUSBFolder
            // 
            this.textBoxUSBFolder.Location = new System.Drawing.Point(66, 283);
            this.textBoxUSBFolder.Name = "textBoxUSBFolder";
            this.textBoxUSBFolder.Size = new System.Drawing.Size(188, 23);
            this.textBoxUSBFolder.TabIndex = 20;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.label46.Location = new System.Drawing.Point(757, 36);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(39, 15);
            this.label46.TabIndex = 16;
            this.label46.Text = "Songs";
            // 
            // comboUSBList
            // 
            this.comboUSBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUSBList.FormattingEnabled = true;
            this.comboUSBList.Location = new System.Drawing.Point(8, 9);
            this.comboUSBList.Name = "comboUSBList";
            this.comboUSBList.Size = new System.Drawing.Size(138, 23);
            this.comboUSBList.TabIndex = 10;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.label45.Location = new System.Drawing.Point(8, 36);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(48, 15);
            this.label45.TabIndex = 9;
            this.label45.Text = "Folders:";
            // 
            // buttonUSBRenameFile
            // 
            this.buttonUSBRenameFile.ImageIndex = 79;
            this.buttonUSBRenameFile.ImageList = this.imageList1;
            this.buttonUSBRenameFile.Location = new System.Drawing.Point(536, 292);
            this.buttonUSBRenameFile.Name = "buttonUSBRenameFile";
            this.buttonUSBRenameFile.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBRenameFile.TabIndex = 37;
            this.toolTip1.SetToolTip(this.buttonUSBRenameFile, "Rename");
            this.buttonUSBRenameFile.UseVisualStyleBackColor = true;
            this.buttonUSBRenameFile.Click += new System.EventHandler(this.buttonUSBRenameFile_Click);
            // 
            // buttonUSBViewPackage
            // 
            this.buttonUSBViewPackage.ImageIndex = 52;
            this.buttonUSBViewPackage.ImageList = this.imageList1;
            this.buttonUSBViewPackage.Location = new System.Drawing.Point(640, 292);
            this.buttonUSBViewPackage.Name = "buttonUSBViewPackage";
            this.buttonUSBViewPackage.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBViewPackage.TabIndex = 35;
            this.toolTip1.SetToolTip(this.buttonUSBViewPackage, "View Package");
            this.buttonUSBViewPackage.UseVisualStyleBackColor = true;
            this.buttonUSBViewPackage.Click += new System.EventHandler(this.buttonUSBViewPackage_Click);
            // 
            // buttonUSBSelectCompletedSongs
            // 
            this.buttonUSBSelectCompletedSongs.ImageIndex = 47;
            this.buttonUSBSelectCompletedSongs.ImageList = this.imageList1;
            this.buttonUSBSelectCompletedSongs.Location = new System.Drawing.Point(812, 293);
            this.buttonUSBSelectCompletedSongs.Name = "buttonUSBSelectCompletedSongs";
            this.buttonUSBSelectCompletedSongs.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBSelectCompletedSongs.TabIndex = 31;
            this.toolTip1.SetToolTip(this.buttonUSBSelectCompletedSongs, "Select Completed");
            this.buttonUSBSelectCompletedSongs.UseVisualStyleBackColor = true;
            // 
            // buttonUSBSelectAllSongs
            // 
            this.buttonUSBSelectAllSongs.ImageIndex = 46;
            this.buttonUSBSelectAllSongs.ImageList = this.imageList1;
            this.buttonUSBSelectAllSongs.Location = new System.Drawing.Point(786, 293);
            this.buttonUSBSelectAllSongs.Name = "buttonUSBSelectAllSongs";
            this.buttonUSBSelectAllSongs.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBSelectAllSongs.TabIndex = 30;
            this.toolTip1.SetToolTip(this.buttonUSBSelectAllSongs, "Select All");
            this.buttonUSBSelectAllSongs.UseVisualStyleBackColor = true;
            this.buttonUSBSelectAllSongs.Click += new System.EventHandler(this.buttonUSBSelectAllSongs_Click);
            // 
            // buttonUSBAddFile
            // 
            this.buttonUSBAddFile.ImageIndex = 35;
            this.buttonUSBAddFile.ImageList = this.imageList1;
            this.buttonUSBAddFile.Location = new System.Drawing.Point(588, 292);
            this.buttonUSBAddFile.Name = "buttonUSBAddFile";
            this.buttonUSBAddFile.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBAddFile.TabIndex = 29;
            this.toolTip1.SetToolTip(this.buttonUSBAddFile, "Add File");
            this.buttonUSBAddFile.UseVisualStyleBackColor = true;
            // 
            // buttonUSBDeleteFile
            // 
            this.buttonUSBDeleteFile.ImageIndex = 32;
            this.buttonUSBDeleteFile.ImageList = this.imageList1;
            this.buttonUSBDeleteFile.Location = new System.Drawing.Point(666, 292);
            this.buttonUSBDeleteFile.Name = "buttonUSBDeleteFile";
            this.buttonUSBDeleteFile.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBDeleteFile.TabIndex = 28;
            this.toolTip1.SetToolTip(this.buttonUSBDeleteFile, "Delete Selected");
            this.buttonUSBDeleteFile.UseVisualStyleBackColor = true;
            // 
            // buttonUSBSetDefaultFolder
            // 
            this.buttonUSBSetDefaultFolder.ImageIndex = 67;
            this.buttonUSBSetDefaultFolder.ImageList = this.imageList1;
            this.buttonUSBSetDefaultFolder.Location = new System.Drawing.Point(285, 305);
            this.buttonUSBSetDefaultFolder.Name = "buttonUSBSetDefaultFolder";
            this.buttonUSBSetDefaultFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBSetDefaultFolder.TabIndex = 23;
            this.toolTip1.SetToolTip(this.buttonUSBSetDefaultFolder, "Set as Default Folder");
            this.buttonUSBSetDefaultFolder.UseVisualStyleBackColor = true;
            // 
            // buttonUSBRenameFolder
            // 
            this.buttonUSBRenameFolder.ImageIndex = 79;
            this.buttonUSBRenameFolder.ImageList = this.imageList1;
            this.buttonUSBRenameFolder.Location = new System.Drawing.Point(259, 280);
            this.buttonUSBRenameFolder.Name = "buttonUSBRenameFolder";
            this.buttonUSBRenameFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBRenameFolder.TabIndex = 22;
            this.toolTip1.SetToolTip(this.buttonUSBRenameFolder, "Rename");
            this.buttonUSBRenameFolder.UseVisualStyleBackColor = true;
            // 
            // buttonUSBCreateFolder
            // 
            this.buttonUSBCreateFolder.ImageIndex = 34;
            this.buttonUSBCreateFolder.ImageList = this.imageList1;
            this.buttonUSBCreateFolder.Location = new System.Drawing.Point(285, 280);
            this.buttonUSBCreateFolder.Name = "buttonUSBCreateFolder";
            this.buttonUSBCreateFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBCreateFolder.TabIndex = 21;
            this.toolTip1.SetToolTip(this.buttonUSBCreateFolder, "Create Folder");
            this.buttonUSBCreateFolder.UseVisualStyleBackColor = true;
            // 
            // buttonUSBRefresh
            // 
            this.buttonUSBRefresh.ImageIndex = 36;
            this.buttonUSBRefresh.ImageList = this.imageList1;
            this.buttonUSBRefresh.Location = new System.Drawing.Point(175, 8);
            this.buttonUSBRefresh.Name = "buttonUSBRefresh";
            this.buttonUSBRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBRefresh.TabIndex = 19;
            this.buttonUSBRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonUSBRefresh, "Refresh");
            this.buttonUSBRefresh.UseVisualStyleBackColor = true;
            this.buttonUSBRefresh.Click += new System.EventHandler(this.buttonUSBRefresh_Click);
            // 
            // buttonUSBCopySelectedSongToUSB
            // 
            this.buttonUSBCopySelectedSongToUSB.ImageIndex = 21;
            this.buttonUSBCopySelectedSongToUSB.ImageList = this.imageList1;
            this.buttonUSBCopySelectedSongToUSB.Location = new System.Drawing.Point(760, 293);
            this.buttonUSBCopySelectedSongToUSB.Name = "buttonUSBCopySelectedSongToUSB";
            this.buttonUSBCopySelectedSongToUSB.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBCopySelectedSongToUSB.TabIndex = 17;
            this.toolTip1.SetToolTip(this.buttonUSBCopySelectedSongToUSB, "Copy to USB");
            this.buttonUSBCopySelectedSongToUSB.UseVisualStyleBackColor = true;
            // 
            // buttonUSBExtractSelectedFiles
            // 
            this.buttonUSBExtractSelectedFiles.ImageIndex = 49;
            this.buttonUSBExtractSelectedFiles.ImageList = this.imageList1;
            this.buttonUSBExtractSelectedFiles.Location = new System.Drawing.Point(562, 292);
            this.buttonUSBExtractSelectedFiles.Name = "buttonUSBExtractSelectedFiles";
            this.buttonUSBExtractSelectedFiles.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBExtractSelectedFiles.TabIndex = 14;
            this.toolTip1.SetToolTip(this.buttonUSBExtractSelectedFiles, "Extract");
            this.buttonUSBExtractSelectedFiles.UseVisualStyleBackColor = true;
            // 
            // buttonUSBExtractFolder
            // 
            this.buttonUSBExtractFolder.ImageIndex = 49;
            this.buttonUSBExtractFolder.ImageList = this.imageList1;
            this.buttonUSBExtractFolder.Location = new System.Drawing.Point(33, 280);
            this.buttonUSBExtractFolder.Name = "buttonUSBExtractFolder";
            this.buttonUSBExtractFolder.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBExtractFolder.TabIndex = 13;
            this.toolTip1.SetToolTip(this.buttonUSBExtractFolder, "Extract Folder");
            this.buttonUSBExtractFolder.UseVisualStyleBackColor = true;
            // 
            // buttonUSBDeleteSelected
            // 
            this.buttonUSBDeleteSelected.ImageIndex = 32;
            this.buttonUSBDeleteSelected.ImageList = this.imageList1;
            this.buttonUSBDeleteSelected.Location = new System.Drawing.Point(6, 280);
            this.buttonUSBDeleteSelected.Name = "buttonUSBDeleteSelected";
            this.buttonUSBDeleteSelected.Size = new System.Drawing.Size(24, 24);
            this.buttonUSBDeleteSelected.TabIndex = 12;
            this.toolTip1.SetToolTip(this.buttonUSBDeleteSelected, "Delete Selected");
            this.buttonUSBDeleteSelected.UseVisualStyleBackColor = true;
            // 
            // button125
            // 
            this.button125.ImageIndex = 19;
            this.button125.ImageList = this.imageList1;
            this.button125.Location = new System.Drawing.Point(149, 8);
            this.button125.Name = "button125";
            this.button125.Size = new System.Drawing.Size(24, 24);
            this.button125.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button125, "Open USB");
            this.button125.UseVisualStyleBackColor = true;
            this.button125.Click += new System.EventHandler(this.button125_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.AutoScroll = true;
            this.tabSettings.AutoScrollMinSize = new System.Drawing.Size(1010, 408);
            this.tabSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tabSettings.Controls.Add(this.groupBox38);
            this.tabSettings.Controls.Add(this.groupBox33);
            this.tabSettings.Controls.Add(this.groupBox20);
            this.tabSettings.Controls.Add(this.groupBox18);
            this.tabSettings.Controls.Add(this.groupBox16);
            this.tabSettings.Controls.Add(this.groupBox17);
            this.tabSettings.Controls.Add(this.groupBox19);
            this.tabSettings.ImageIndex = 30;
            this.tabSettings.Location = new System.Drawing.Point(4, 23);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1095, 523);
            this.tabSettings.TabIndex = 5;
            this.tabSettings.Text = "Settings";
            // 
            // groupBox38
            // 
            this.groupBox38.Controls.Add(this.button115);
            this.groupBox38.Controls.Add(this.button114);
            this.groupBox38.Controls.Add(this.button112);
            this.groupBox38.Controls.Add(this.button111);
            this.groupBox38.Controls.Add(this.comboBoxMidiInput);
            this.groupBox38.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox38.Location = new System.Drawing.Point(449, 133);
            this.groupBox38.Name = "groupBox38";
            this.groupBox38.Size = new System.Drawing.Size(212, 108);
            this.groupBox38.TabIndex = 6;
            this.groupBox38.TabStop = false;
            this.groupBox38.Text = "Midi Input (Mustang/Squier)";
            // 
            // button115
            // 
            this.button115.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button115.Location = new System.Drawing.Point(145, 76);
            this.button115.Name = "button115";
            this.button115.Size = new System.Drawing.Size(57, 23);
            this.button115.TabIndex = 7;
            this.button115.Text = "Unhook";
            this.button115.UseVisualStyleBackColor = true;
            this.button115.Click += new System.EventHandler(this.button115_Click);
            // 
            // button114
            // 
            this.button114.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button114.Location = new System.Drawing.Point(6, 76);
            this.button114.Name = "button114";
            this.button114.Size = new System.Drawing.Size(133, 23);
            this.button114.TabIndex = 6;
            this.button114.Text = "Hook directly to midi out";
            this.button114.UseVisualStyleBackColor = true;
            this.button114.Click += new System.EventHandler(this.button114_Click);
            // 
            // button112
            // 
            this.button112.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button112.Location = new System.Drawing.Point(64, 47);
            this.button112.Name = "button112";
            this.button112.Size = new System.Drawing.Size(75, 23);
            this.button112.TabIndex = 5;
            this.button112.Text = "Refresh";
            this.button112.UseVisualStyleBackColor = true;
            this.button112.Click += new System.EventHandler(this.button112_Click);
            // 
            // button111
            // 
            this.button111.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button111.Location = new System.Drawing.Point(145, 47);
            this.button111.Name = "button111";
            this.button111.Size = new System.Drawing.Size(57, 23);
            this.button111.TabIndex = 4;
            this.button111.Text = "Update";
            this.button111.UseVisualStyleBackColor = true;
            this.button111.Click += new System.EventHandler(this.button111_Click);
            // 
            // comboBoxMidiInput
            // 
            this.comboBoxMidiInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMidiInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBoxMidiInput.FormattingEnabled = true;
            this.comboBoxMidiInput.Location = new System.Drawing.Point(9, 20);
            this.comboBoxMidiInput.Name = "comboBoxMidiInput";
            this.comboBoxMidiInput.Size = new System.Drawing.Size(193, 23);
            this.comboBoxMidiInput.TabIndex = 0;
            // 
            // groupBox33
            // 
            this.groupBox33.Controls.Add(this.button113);
            this.groupBox33.Controls.Add(this.button110);
            this.groupBox33.Controls.Add(this.comboMidiDevice);
            this.groupBox33.Controls.Add(this.label14);
            this.groupBox33.Controls.Add(this.button70);
            this.groupBox33.Controls.Add(this.comboMidiInstrument);
            this.groupBox33.Controls.Add(this.label12);
            this.groupBox33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox33.Location = new System.Drawing.Point(449, 247);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Size = new System.Drawing.Size(212, 111);
            this.groupBox33.TabIndex = 4;
            this.groupBox33.TabStop = false;
            this.groupBox33.Text = "MIDI Playback";
            // 
            // button113
            // 
            this.button113.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button113.Location = new System.Drawing.Point(92, 73);
            this.button113.Name = "button113";
            this.button113.Size = new System.Drawing.Size(55, 23);
            this.button113.TabIndex = 6;
            this.button113.Text = "Refresh";
            this.button113.UseVisualStyleBackColor = true;
            this.button113.Click += new System.EventHandler(this.button113_Click);
            // 
            // button110
            // 
            this.button110.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button110.Location = new System.Drawing.Point(24, 73);
            this.button110.Name = "button110";
            this.button110.Size = new System.Drawing.Size(62, 23);
            this.button110.TabIndex = 2;
            this.button110.Text = "Preview";
            this.button110.UseVisualStyleBackColor = true;
            this.button110.Click += new System.EventHandler(this.button110_Click);
            // 
            // comboMidiDevice
            // 
            this.comboMidiDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMidiDevice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboMidiDevice.FormattingEnabled = true;
            this.comboMidiDevice.Location = new System.Drawing.Point(80, 19);
            this.comboMidiDevice.Name = "comboMidiDevice";
            this.comboMidiDevice.Size = new System.Drawing.Size(126, 23);
            this.comboMidiDevice.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(29, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 15);
            this.label14.TabIndex = 3;
            this.label14.Text = "Device:";
            // 
            // button70
            // 
            this.button70.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button70.Location = new System.Drawing.Point(153, 73);
            this.button70.Name = "button70";
            this.button70.Size = new System.Drawing.Size(53, 23);
            this.button70.TabIndex = 3;
            this.button70.Text = "Update";
            this.button70.UseVisualStyleBackColor = true;
            this.button70.Click += new System.EventHandler(this.button70_Click);
            // 
            // comboMidiInstrument
            // 
            this.comboMidiInstrument.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMidiInstrument.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboMidiInstrument.FormattingEnabled = true;
            this.comboMidiInstrument.Location = new System.Drawing.Point(80, 46);
            this.comboMidiInstrument.Name = "comboMidiInstrument";
            this.comboMidiInstrument.Size = new System.Drawing.Size(126, 23);
            this.comboMidiInstrument.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(6, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Instrument:";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.vScrollBar1);
            this.groupBox20.Controls.Add(this.panel7);
            this.groupBox20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox20.Location = new System.Drawing.Point(667, 136);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(397, 265);
            this.groupBox20.TabIndex = 5;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Internal Configuration Properties";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.LargeChange = 20;
            this.vScrollBar1.Location = new System.Drawing.Point(370, 18);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(19, 237);
            this.vScrollBar1.SmallChange = 10;
            this.vScrollBar1.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.panel6);
            this.panel7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel7.Location = new System.Drawing.Point(7, 16);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(383, 240);
            this.panel7.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(362, 238);
            this.panel6.TabIndex = 0;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.checkView5Button);
            this.groupBox18.Controls.Add(this.checkBoxMidiInputStartup);
            this.groupBox18.Controls.Add(this.checkBoxEnableMidiInput);
            this.groupBox18.Controls.Add(this.textBoxNoteCloseDist);
            this.groupBox18.Controls.Add(this.label44);
            this.groupBox18.Controls.Add(this.textClearHoldBox);
            this.groupBox18.Controls.Add(this.label25);
            this.groupBox18.Controls.Add(this.textBoxMidiScrollFreq);
            this.groupBox18.Controls.Add(this.label22);
            this.groupBox18.Controls.Add(this.checkBoxLoadLastSongStartup);
            this.groupBox18.Controls.Add(this.checkBoxMidiPlaybackScroll);
            this.groupBox18.Controls.Add(this.button57);
            this.groupBox18.Controls.Add(this.label5);
            this.groupBox18.Controls.Add(this.textBoxMinimumNoteWidth);
            this.groupBox18.Controls.Add(this.label23);
            this.groupBox18.Controls.Add(this.textBoxScrollToSelectionOffset);
            this.groupBox18.Controls.Add(this.checkBoxShowMidiChannelEdit);
            this.groupBox18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox18.Location = new System.Drawing.Point(6, 6);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(437, 190);
            this.groupBox18.TabIndex = 0;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Editor Configuration";
            // 
            // checkView5Button
            // 
            this.checkView5Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkView5Button.Location = new System.Drawing.Point(10, 114);
            this.checkView5Button.Name = "checkView5Button";
            this.checkView5Button.Size = new System.Drawing.Size(157, 18);
            this.checkView5Button.TabIndex = 52;
            this.checkView5Button.Text = "View 5 Button";
            this.checkView5Button.UseCompatibleTextRendering = true;
            this.checkView5Button.UseVisualStyleBackColor = true;
            // 
            // checkBoxMidiInputStartup
            // 
            this.checkBoxMidiInputStartup.Checked = true;
            this.checkBoxMidiInputStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMidiInputStartup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMidiInputStartup.Location = new System.Drawing.Point(10, 68);
            this.checkBoxMidiInputStartup.Name = "checkBoxMidiInputStartup";
            this.checkBoxMidiInputStartup.Size = new System.Drawing.Size(198, 18);
            this.checkBoxMidiInputStartup.TabIndex = 51;
            this.checkBoxMidiInputStartup.Text = "Connect Midi Input on Startup";
            this.checkBoxMidiInputStartup.UseCompatibleTextRendering = true;
            this.checkBoxMidiInputStartup.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableMidiInput
            // 
            this.checkBoxEnableMidiInput.Checked = true;
            this.checkBoxEnableMidiInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableMidiInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxEnableMidiInput.Location = new System.Drawing.Point(10, 45);
            this.checkBoxEnableMidiInput.Name = "checkBoxEnableMidiInput";
            this.checkBoxEnableMidiInput.Size = new System.Drawing.Size(191, 18);
            this.checkBoxEnableMidiInput.TabIndex = 50;
            this.checkBoxEnableMidiInput.Text = "Enable Midi Input Instrument";
            this.checkBoxEnableMidiInput.UseCompatibleTextRendering = true;
            this.checkBoxEnableMidiInput.UseVisualStyleBackColor = true;
            this.checkBoxEnableMidiInput.CheckedChanged += new System.EventHandler(this.checkBoxEnableMidiInput_CheckedChanged);
            // 
            // textBoxNoteCloseDist
            // 
            this.textBoxNoteCloseDist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNoteCloseDist.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteCloseDist.Location = new System.Drawing.Point(379, 123);
            this.textBoxNoteCloseDist.Name = "textBoxNoteCloseDist";
            this.textBoxNoteCloseDist.Size = new System.Drawing.Size(52, 23);
            this.textBoxNoteCloseDist.TabIndex = 48;
            this.textBoxNoteCloseDist.Text = "8";
            // 
            // label44
            // 
            this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label44.Location = new System.Drawing.Point(282, 126);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(91, 15);
            this.label44.TabIndex = 49;
            this.label44.Text = "Note Close Dist:";
            // 
            // textClearHoldBox
            // 
            this.textClearHoldBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textClearHoldBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textClearHoldBox.Location = new System.Drawing.Point(379, 97);
            this.textClearHoldBox.Name = "textClearHoldBox";
            this.textClearHoldBox.Size = new System.Drawing.Size(52, 23);
            this.textClearHoldBox.TabIndex = 48;
            this.textClearHoldBox.Text = "1";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label25.Location = new System.Drawing.Point(259, 101);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(114, 15);
            this.label25.TabIndex = 49;
            this.label25.Text = "Clear Hold Box Secs:";
            // 
            // textBoxMidiScrollFreq
            // 
            this.textBoxMidiScrollFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMidiScrollFreq.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxMidiScrollFreq.Location = new System.Drawing.Point(379, 71);
            this.textBoxMidiScrollFreq.Name = "textBoxMidiScrollFreq";
            this.textBoxMidiScrollFreq.Size = new System.Drawing.Size(52, 23);
            this.textBoxMidiScrollFreq.TabIndex = 4;
            this.textBoxMidiScrollFreq.Text = "5";
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(246, 74);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(127, 15);
            this.label22.TabIndex = 47;
            this.label22.Text = "Midi Scroll Update MS:";
            // 
            // checkBoxLoadLastSongStartup
            // 
            this.checkBoxLoadLastSongStartup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxLoadLastSongStartup.Location = new System.Drawing.Point(10, 91);
            this.checkBoxLoadLastSongStartup.Name = "checkBoxLoadLastSongStartup";
            this.checkBoxLoadLastSongStartup.Size = new System.Drawing.Size(182, 18);
            this.checkBoxLoadLastSongStartup.TabIndex = 2;
            this.checkBoxLoadLastSongStartup.Text = "Load Last Song on Startup";
            this.checkBoxLoadLastSongStartup.UseCompatibleTextRendering = true;
            this.checkBoxLoadLastSongStartup.UseVisualStyleBackColor = true;
            // 
            // checkBoxMidiPlaybackScroll
            // 
            this.checkBoxMidiPlaybackScroll.Checked = true;
            this.checkBoxMidiPlaybackScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMidiPlaybackScroll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxMidiPlaybackScroll.Location = new System.Drawing.Point(10, 23);
            this.checkBoxMidiPlaybackScroll.Name = "checkBoxMidiPlaybackScroll";
            this.checkBoxMidiPlaybackScroll.Size = new System.Drawing.Size(189, 18);
            this.checkBoxMidiPlaybackScroll.TabIndex = 3;
            this.checkBoxMidiPlaybackScroll.Text = "Midi Playback Scroll";
            this.checkBoxMidiPlaybackScroll.UseCompatibleTextRendering = true;
            this.checkBoxMidiPlaybackScroll.UseVisualStyleBackColor = true;
            // 
            // button57
            // 
            this.button57.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button57.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button57.ImageIndex = 65;
            this.button57.ImageList = this.imageList1;
            this.button57.Location = new System.Drawing.Point(407, 152);
            this.button57.Name = "button57";
            this.button57.Size = new System.Drawing.Size(24, 24);
            this.button57.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button57, "Update Changes");
            this.button57.UseVisualStyleBackColor = true;
            this.button57.Click += new System.EventHandler(this.button57_Click_3);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(246, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "Minimum Note Width:";
            // 
            // textBoxMinimumNoteWidth
            // 
            this.textBoxMinimumNoteWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMinimumNoteWidth.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxMinimumNoteWidth.Location = new System.Drawing.Point(379, 47);
            this.textBoxMinimumNoteWidth.Name = "textBoxMinimumNoteWidth";
            this.textBoxMinimumNoteWidth.Size = new System.Drawing.Size(52, 23);
            this.textBoxMinimumNoteWidth.TabIndex = 1;
            this.textBoxMinimumNoteWidth.Text = "0";
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label23.Location = new System.Drawing.Point(231, 24);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(142, 15);
            this.label23.TabIndex = 14;
            this.label23.Text = "Scroll To Selection Offset:";
            // 
            // textBoxScrollToSelectionOffset
            // 
            this.textBoxScrollToSelectionOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScrollToSelectionOffset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxScrollToSelectionOffset.Location = new System.Drawing.Point(379, 21);
            this.textBoxScrollToSelectionOffset.Name = "textBoxScrollToSelectionOffset";
            this.textBoxScrollToSelectionOffset.Size = new System.Drawing.Size(52, 23);
            this.textBoxScrollToSelectionOffset.TabIndex = 0;
            this.textBoxScrollToSelectionOffset.Text = "300";
            // 
            // checkBoxShowMidiChannelEdit
            // 
            this.checkBoxShowMidiChannelEdit.Checked = true;
            this.checkBoxShowMidiChannelEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowMidiChannelEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxShowMidiChannelEdit.Location = new System.Drawing.Point(10, 137);
            this.checkBoxShowMidiChannelEdit.Name = "checkBoxShowMidiChannelEdit";
            this.checkBoxShowMidiChannelEdit.Size = new System.Drawing.Size(190, 18);
            this.checkBoxShowMidiChannelEdit.TabIndex = 13;
            this.checkBoxShowMidiChannelEdit.Text = "View Note Channels";
            this.checkBoxShowMidiChannelEdit.UseCompatibleTextRendering = true;
            this.checkBoxShowMidiChannelEdit.UseVisualStyleBackColor = true;
            this.checkBoxShowMidiChannelEdit.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged_1);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.textBoxDefaultCONFileLocation);
            this.groupBox16.Controls.Add(this.textBoxDefaultMidi5FileLocation);
            this.groupBox16.Controls.Add(this.textBoxDefaultMidiProFileLocation);
            this.groupBox16.Controls.Add(this.label31);
            this.groupBox16.Controls.Add(this.button62);
            this.groupBox16.Controls.Add(this.checkUseDefaultFolders);
            this.groupBox16.Controls.Add(this.label32);
            this.groupBox16.Controls.Add(this.button64);
            this.groupBox16.Controls.Add(this.button63);
            this.groupBox16.Controls.Add(this.label33);
            this.groupBox16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox16.Location = new System.Drawing.Point(605, 6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(455, 124);
            this.groupBox16.TabIndex = 1;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Default Folders";
            // 
            // textBoxDefaultCONFileLocation
            // 
            this.textBoxDefaultCONFileLocation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxDefaultCONFileLocation.Location = new System.Drawing.Point(166, 19);
            this.textBoxDefaultCONFileLocation.Name = "textBoxDefaultCONFileLocation";
            this.textBoxDefaultCONFileLocation.Size = new System.Drawing.Size(242, 23);
            this.textBoxDefaultCONFileLocation.TabIndex = 0;
            // 
            // textBoxDefaultMidi5FileLocation
            // 
            this.textBoxDefaultMidi5FileLocation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxDefaultMidi5FileLocation.Location = new System.Drawing.Point(166, 45);
            this.textBoxDefaultMidi5FileLocation.Name = "textBoxDefaultMidi5FileLocation";
            this.textBoxDefaultMidi5FileLocation.Size = new System.Drawing.Size(242, 23);
            this.textBoxDefaultMidi5FileLocation.TabIndex = 2;
            // 
            // textBoxDefaultMidiProFileLocation
            // 
            this.textBoxDefaultMidiProFileLocation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxDefaultMidiProFileLocation.Location = new System.Drawing.Point(166, 71);
            this.textBoxDefaultMidiProFileLocation.Name = "textBoxDefaultMidiProFileLocation";
            this.textBoxDefaultMidiProFileLocation.Size = new System.Drawing.Size(242, 23);
            this.textBoxDefaultMidiProFileLocation.TabIndex = 4;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label31.Location = new System.Drawing.Point(25, 22);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(138, 15);
            this.label31.TabIndex = 3;
            this.label31.Text = "Open CON File Location:";
            this.label31.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button62
            // 
            this.button62.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button62.ImageIndex = 0;
            this.button62.ImageList = this.imageList1;
            this.button62.Location = new System.Drawing.Point(413, 17);
            this.button62.Name = "button62";
            this.button62.Size = new System.Drawing.Size(29, 23);
            this.button62.TabIndex = 1;
            this.button62.UseVisualStyleBackColor = true;
            this.button62.Click += new System.EventHandler(this.button62_Click);
            // 
            // checkUseDefaultFolders
            // 
            this.checkUseDefaultFolders.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkUseDefaultFolders.Location = new System.Drawing.Point(166, 100);
            this.checkUseDefaultFolders.Name = "checkUseDefaultFolders";
            this.checkUseDefaultFolders.Size = new System.Drawing.Size(201, 18);
            this.checkUseDefaultFolders.TabIndex = 6;
            this.checkUseDefaultFolders.Text = "Use Default Folders";
            this.checkUseDefaultFolders.UseCompatibleTextRendering = true;
            this.checkUseDefaultFolders.UseVisualStyleBackColor = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label32.Location = new System.Drawing.Point(18, 48);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(145, 15);
            this.label32.TabIndex = 6;
            this.label32.Text = "Open Midi 5 File Location:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button64
            // 
            this.button64.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button64.ImageIndex = 0;
            this.button64.ImageList = this.imageList1;
            this.button64.Location = new System.Drawing.Point(413, 69);
            this.button64.Name = "button64";
            this.button64.Size = new System.Drawing.Size(29, 23);
            this.button64.TabIndex = 5;
            this.button64.UseVisualStyleBackColor = true;
            this.button64.Click += new System.EventHandler(this.button64_Click);
            // 
            // button63
            // 
            this.button63.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button63.ImageIndex = 0;
            this.button63.ImageList = this.imageList1;
            this.button63.Location = new System.Drawing.Point(413, 43);
            this.button63.Name = "button63";
            this.button63.Size = new System.Drawing.Size(29, 23);
            this.button63.TabIndex = 3;
            this.button63.UseVisualStyleBackColor = true;
            this.button63.Click += new System.EventHandler(this.button63_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label33.Location = new System.Drawing.Point(6, 74);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(157, 15);
            this.label33.TabIndex = 6;
            this.label33.Text = "Open Midi Pro File Location:";
            this.label33.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.label19);
            this.groupBox17.Controls.Add(this.textBoxZoom);
            this.groupBox17.Controls.Add(this.button27);
            this.groupBox17.Controls.Add(this.button84);
            this.groupBox17.Controls.Add(this.button83);
            this.groupBox17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox17.Location = new System.Drawing.Point(449, 6);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(150, 85);
            this.groupBox17.TabIndex = 3;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Note Zoom";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(5, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(42, 15);
            this.label19.TabIndex = 0;
            this.label19.Text = "Zoom:";
            // 
            // textBoxZoom
            // 
            this.textBoxZoom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxZoom.Location = new System.Drawing.Point(53, 21);
            this.textBoxZoom.Name = "textBoxZoom";
            this.textBoxZoom.Size = new System.Drawing.Size(86, 23);
            this.textBoxZoom.TabIndex = 0;
            // 
            // button27
            // 
            this.button27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button27.ImageIndex = 36;
            this.button27.ImageList = this.imageList1;
            this.button27.Location = new System.Drawing.Point(115, 48);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(24, 24);
            this.button27.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button27, "Update");
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // button84
            // 
            this.button84.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button84.ImageIndex = 85;
            this.button84.ImageList = this.imageList1;
            this.button84.Location = new System.Drawing.Point(53, 48);
            this.button84.Name = "button84";
            this.button84.Size = new System.Drawing.Size(24, 24);
            this.button84.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button84, "Zoom In");
            this.button84.UseVisualStyleBackColor = true;
            this.button84.Click += new System.EventHandler(this.button84_Click);
            // 
            // button83
            // 
            this.button83.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button83.ImageIndex = 86;
            this.button83.ImageList = this.imageList1;
            this.button83.Location = new System.Drawing.Point(78, 48);
            this.button83.Name = "button83";
            this.button83.Size = new System.Drawing.Size(24, 24);
            this.button83.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button83, "Zoom Out");
            this.button83.UseVisualStyleBackColor = true;
            this.button83.Click += new System.EventHandler(this.button83_Click);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.radioGridHalfNote);
            this.groupBox19.Controls.Add(this.radioGridWholeNote);
            this.groupBox19.Controls.Add(this.checkSnapToCloseG5);
            this.groupBox19.Controls.Add(this.button119);
            this.groupBox19.Controls.Add(this.textBoxNoteSnapDistance);
            this.groupBox19.Controls.Add(this.label43);
            this.groupBox19.Controls.Add(this.textBoxGridSnapDistance);
            this.groupBox19.Controls.Add(this.label26);
            this.groupBox19.Controls.Add(this.checkBoxRenderMouseSnap);
            this.groupBox19.Controls.Add(this.checkBoxSnapToCloseNotes);
            this.groupBox19.Controls.Add(this.radioGrid128thNote);
            this.groupBox19.Controls.Add(this.checkBoxGridSnap);
            this.groupBox19.Controls.Add(this.radioGrid32Note);
            this.groupBox19.Controls.Add(this.checkViewNotesGrid5Button);
            this.groupBox19.Controls.Add(this.checkViewNotesGridPro);
            this.groupBox19.Controls.Add(this.radioGrid16Note);
            this.groupBox19.Controls.Add(this.radioGrid64thNote);
            this.groupBox19.Controls.Add(this.radioGrid8Note);
            this.groupBox19.Controls.Add(this.radioGridQuarterNote);
            this.groupBox19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox19.Location = new System.Drawing.Point(7, 202);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(436, 217);
            this.groupBox19.TabIndex = 2;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Notes Grid";
            // 
            // radioGridHalfNote
            // 
            this.radioGridHalfNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGridHalfNote.Location = new System.Drawing.Point(24, 45);
            this.radioGridHalfNote.Name = "radioGridHalfNote";
            this.radioGridHalfNote.Size = new System.Drawing.Size(51, 17);
            this.radioGridHalfNote.TabIndex = 54;
            this.radioGridHalfNote.Text = "Half";
            this.radioGridHalfNote.UseVisualStyleBackColor = true;
            this.radioGridHalfNote.CheckedChanged += new System.EventHandler(this.radioGridHalfNote_CheckedChanged);
            // 
            // radioGridWholeNote
            // 
            this.radioGridWholeNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGridWholeNote.Location = new System.Drawing.Point(90, 45);
            this.radioGridWholeNote.Name = "radioGridWholeNote";
            this.radioGridWholeNote.Size = new System.Drawing.Size(72, 19);
            this.radioGridWholeNote.TabIndex = 55;
            this.radioGridWholeNote.Text = "Whole";
            this.radioGridWholeNote.UseVisualStyleBackColor = true;
            this.radioGridWholeNote.CheckedChanged += new System.EventHandler(this.radioGridWholeNote_CheckedChanged);
            // 
            // checkSnapToCloseG5
            // 
            this.checkSnapToCloseG5.Checked = true;
            this.checkSnapToCloseG5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSnapToCloseG5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkSnapToCloseG5.Location = new System.Drawing.Point(159, 132);
            this.checkSnapToCloseG5.Name = "checkSnapToCloseG5";
            this.checkSnapToCloseG5.Size = new System.Drawing.Size(144, 19);
            this.checkSnapToCloseG5.TabIndex = 53;
            this.checkSnapToCloseG5.Text = "Snap To Close G5 Notes";
            this.checkSnapToCloseG5.UseVisualStyleBackColor = true;
            // 
            // button119
            // 
            this.button119.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button119.ImageIndex = 65;
            this.button119.ImageList = this.imageList1;
            this.button119.Location = new System.Drawing.Point(406, 179);
            this.button119.Name = "button119";
            this.button119.Size = new System.Drawing.Size(24, 24);
            this.button119.TabIndex = 52;
            this.toolTip1.SetToolTip(this.button119, "Update Changes");
            this.button119.UseVisualStyleBackColor = true;
            this.button119.Click += new System.EventHandler(this.button119_Click);
            // 
            // textBoxNoteSnapDistance
            // 
            this.textBoxNoteSnapDistance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNoteSnapDistance.Location = new System.Drawing.Point(120, 181);
            this.textBoxNoteSnapDistance.Name = "textBoxNoteSnapDistance";
            this.textBoxNoteSnapDistance.Size = new System.Drawing.Size(52, 23);
            this.textBoxNoteSnapDistance.TabIndex = 50;
            this.textBoxNoteSnapDistance.Text = "4";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label43.Location = new System.Drawing.Point(6, 184);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(113, 15);
            this.label43.TabIndex = 51;
            this.label43.Text = "Note Snap Distance:";
            // 
            // textBoxGridSnapDistance
            // 
            this.textBoxGridSnapDistance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxGridSnapDistance.Location = new System.Drawing.Point(120, 155);
            this.textBoxGridSnapDistance.Name = "textBoxGridSnapDistance";
            this.textBoxGridSnapDistance.Size = new System.Drawing.Size(52, 23);
            this.textBoxGridSnapDistance.TabIndex = 50;
            this.textBoxGridSnapDistance.Text = "4";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Location = new System.Drawing.Point(10, 159);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(109, 15);
            this.label26.TabIndex = 51;
            this.label26.Text = "Grid Snap Distance:";
            // 
            // checkBoxRenderMouseSnap
            // 
            this.checkBoxRenderMouseSnap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxRenderMouseSnap.Location = new System.Drawing.Point(159, 71);
            this.checkBoxRenderMouseSnap.Name = "checkBoxRenderMouseSnap";
            this.checkBoxRenderMouseSnap.Size = new System.Drawing.Size(144, 19);
            this.checkBoxRenderMouseSnap.TabIndex = 10;
            this.checkBoxRenderMouseSnap.Text = "Render Mouse Snap";
            this.checkBoxRenderMouseSnap.UseVisualStyleBackColor = true;
            // 
            // checkBoxSnapToCloseNotes
            // 
            this.checkBoxSnapToCloseNotes.Checked = true;
            this.checkBoxSnapToCloseNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSnapToCloseNotes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxSnapToCloseNotes.Location = new System.Drawing.Point(159, 109);
            this.checkBoxSnapToCloseNotes.Name = "checkBoxSnapToCloseNotes";
            this.checkBoxSnapToCloseNotes.Size = new System.Drawing.Size(144, 19);
            this.checkBoxSnapToCloseNotes.TabIndex = 9;
            this.checkBoxSnapToCloseNotes.Text = "Snap To Close Notes";
            this.checkBoxSnapToCloseNotes.UseVisualStyleBackColor = true;
            // 
            // radioGrid128thNote
            // 
            this.radioGrid128thNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGrid128thNote.Location = new System.Drawing.Point(24, 22);
            this.radioGrid128thNote.Name = "radioGrid128thNote";
            this.radioGrid128thNote.Size = new System.Drawing.Size(60, 17);
            this.radioGrid128thNote.TabIndex = 2;
            this.radioGrid128thNote.Text = "128th";
            this.radioGrid128thNote.UseVisualStyleBackColor = true;
            this.radioGrid128thNote.CheckedChanged += new System.EventHandler(this.radioGrid128thNote_CheckedChanged);
            // 
            // checkBoxGridSnap
            // 
            this.checkBoxGridSnap.Checked = true;
            this.checkBoxGridSnap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGridSnap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkBoxGridSnap.Location = new System.Drawing.Point(8, 71);
            this.checkBoxGridSnap.Name = "checkBoxGridSnap";
            this.checkBoxGridSnap.Size = new System.Drawing.Size(145, 17);
            this.checkBoxGridSnap.TabIndex = 8;
            this.checkBoxGridSnap.Text = "Enable Grid Snap";
            this.checkBoxGridSnap.UseVisualStyleBackColor = true;
            // 
            // radioGrid32Note
            // 
            this.radioGrid32Note.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGrid32Note.Location = new System.Drawing.Point(147, 22);
            this.radioGrid32Note.Name = "radioGrid32Note";
            this.radioGrid32Note.Size = new System.Drawing.Size(66, 17);
            this.radioGrid32Note.TabIndex = 4;
            this.radioGrid32Note.Text = "32nd";
            this.radioGrid32Note.UseVisualStyleBackColor = true;
            this.radioGrid32Note.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // checkViewNotesGrid5Button
            // 
            this.checkViewNotesGrid5Button.Checked = true;
            this.checkViewNotesGrid5Button.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkViewNotesGrid5Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkViewNotesGrid5Button.Location = new System.Drawing.Point(8, 109);
            this.checkViewNotesGrid5Button.Name = "checkViewNotesGrid5Button";
            this.checkViewNotesGrid5Button.Size = new System.Drawing.Size(145, 17);
            this.checkViewNotesGrid5Button.TabIndex = 0;
            this.checkViewNotesGrid5Button.Text = "View Notes Grid 5 Button";
            this.checkViewNotesGrid5Button.UseVisualStyleBackColor = true;
            this.checkViewNotesGrid5Button.CheckedChanged += new System.EventHandler(this.checkViewNotesGrid_CheckedChanged);
            // 
            // checkViewNotesGridPro
            // 
            this.checkViewNotesGridPro.Checked = true;
            this.checkViewNotesGridPro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkViewNotesGridPro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkViewNotesGridPro.Location = new System.Drawing.Point(8, 132);
            this.checkViewNotesGridPro.Name = "checkViewNotesGridPro";
            this.checkViewNotesGridPro.Size = new System.Drawing.Size(145, 17);
            this.checkViewNotesGridPro.TabIndex = 1;
            this.checkViewNotesGridPro.Text = "View Notes Grid Pro";
            this.checkViewNotesGridPro.UseVisualStyleBackColor = true;
            this.checkViewNotesGridPro.CheckedChanged += new System.EventHandler(this.checkViewNotesGrid_CheckedChanged);
            // 
            // radioGrid16Note
            // 
            this.radioGrid16Note.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGrid16Note.Location = new System.Drawing.Point(219, 22);
            this.radioGrid16Note.Name = "radioGrid16Note";
            this.radioGrid16Note.Size = new System.Drawing.Size(53, 17);
            this.radioGrid16Note.TabIndex = 5;
            this.radioGrid16Note.Text = "16th";
            this.radioGrid16Note.UseVisualStyleBackColor = true;
            this.radioGrid16Note.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioGrid64thNote
            // 
            this.radioGrid64thNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGrid64thNote.Location = new System.Drawing.Point(90, 22);
            this.radioGrid64thNote.Name = "radioGrid64thNote";
            this.radioGrid64thNote.Size = new System.Drawing.Size(51, 17);
            this.radioGrid64thNote.TabIndex = 3;
            this.radioGrid64thNote.Text = "64th";
            this.radioGrid64thNote.UseVisualStyleBackColor = true;
            this.radioGrid64thNote.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioGrid8Note
            // 
            this.radioGrid8Note.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGrid8Note.Location = new System.Drawing.Point(278, 22);
            this.radioGrid8Note.Name = "radioGrid8Note";
            this.radioGrid8Note.Size = new System.Drawing.Size(51, 17);
            this.radioGrid8Note.TabIndex = 6;
            this.radioGrid8Note.Text = "8th";
            this.radioGrid8Note.UseVisualStyleBackColor = true;
            this.radioGrid8Note.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioGridQuarterNote
            // 
            this.radioGridQuarterNote.Checked = true;
            this.radioGridQuarterNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGridQuarterNote.Location = new System.Drawing.Point(335, 22);
            this.radioGridQuarterNote.Name = "radioGridQuarterNote";
            this.radioGridQuarterNote.Size = new System.Drawing.Size(72, 19);
            this.radioGridQuarterNote.TabIndex = 7;
            this.radioGridQuarterNote.TabStop = true;
            this.radioGridQuarterNote.Text = "Quarter";
            this.radioGridQuarterNote.UseVisualStyleBackColor = true;
            this.radioGridQuarterNote.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // buttonCloseG6Track
            // 
            this.buttonCloseG6Track.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseG6Track.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseG6Track.ForeColor = System.Drawing.Color.Gainsboro;
            this.buttonCloseG6Track.ImageKey = "close.png";
            this.buttonCloseG6Track.ImageList = this.imageList1;
            this.buttonCloseG6Track.Location = new System.Drawing.Point(1077, -2);
            this.buttonCloseG6Track.Name = "buttonCloseG6Track";
            this.buttonCloseG6Track.Size = new System.Drawing.Size(23, 21);
            this.buttonCloseG6Track.TabIndex = 13;
            this.toolTip1.SetToolTip(this.buttonCloseG6Track, "Close");
            this.buttonCloseG6Track.UseVisualStyleBackColor = false;
            this.buttonCloseG6Track.Click += new System.EventHandler(this.buttonCloseG6Track_Click);
            // 
            // buttonCloseG5Track
            // 
            this.buttonCloseG5Track.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseG5Track.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseG5Track.ForeColor = System.Drawing.Color.Gainsboro;
            this.buttonCloseG5Track.ImageKey = "close.png";
            this.buttonCloseG5Track.ImageList = this.imageList1;
            this.buttonCloseG5Track.Location = new System.Drawing.Point(1077, -2);
            this.buttonCloseG5Track.Name = "buttonCloseG5Track";
            this.buttonCloseG5Track.Size = new System.Drawing.Size(23, 21);
            this.buttonCloseG5Track.TabIndex = 12;
            this.toolTip1.SetToolTip(this.buttonCloseG5Track, "Close");
            this.buttonCloseG5Track.UseVisualStyleBackColor = false;
            this.buttonCloseG5Track.Click += new System.EventHandler(this.buttonCloseG5Track_Click);
            // 
            // timerMidiPlayback
            // 
            this.timerMidiPlayback.Interval = 50;
            this.timerMidiPlayback.Tick += new System.EventHandler(this.timerMidiPlayback_Tick);
            // 
            // panel5ButtonEditor
            // 
            this.panel5ButtonEditor.BackColor = System.Drawing.Color.Transparent;
            this.panel5ButtonEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5ButtonEditor.Controls.Add(this.trackEditorG5);
            this.panel5ButtonEditor.Controls.Add(this.panelTrackEditorG5TitleBar);
            this.panel5ButtonEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5ButtonEditor.Location = new System.Drawing.Point(0, 24);
            this.panel5ButtonEditor.Margin = new System.Windows.Forms.Padding(0);
            this.panel5ButtonEditor.Name = "panel5ButtonEditor";
            this.panel5ButtonEditor.Size = new System.Drawing.Size(1103, 139);
            this.panel5ButtonEditor.TabIndex = 13;
            // 
            // trackEditorG5
            // 
            this.trackEditorG5.AllowDrop = true;
            this.trackEditorG5.CurrentDifficulty = ProUpgradeEditor.Common.GuitarDifficulty.Expert;
            this.trackEditorG5.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackEditorG5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackEditorG5.EditMode = ProUpgradeEditor.Common.TrackEditor.EEditMode.Chords;
            this.trackEditorG5.Editor5 = null;
            this.trackEditorG5.EditorPro = null;
            this.trackEditorG5.EditorType = ProUpgradeEditor.Common.TrackEditor.EEditorType.None;
            this.trackEditorG5.EnableRenderMP3Wave = false;
            this.trackEditorG5.GridScalar = 0.25D;
            this.trackEditorG5.GridSnap = true;
            this.trackEditorG5.InPlayback = false;
            this.trackEditorG5.IsMouseOver = false;
            this.trackEditorG5.IsSelecting = false;
            this.trackEditorG5.LoadedFileName = null;
            this.trackEditorG5.Location = new System.Drawing.Point(0, 20);
            this.trackEditorG5.Margin = new System.Windows.Forms.Padding(0);
            this.trackEditorG5.MidiPlaybackPosition = 0;
            this.trackEditorG5.MP3PlaybackOffset = 0;
            
            this.trackEditorG5.Name = "trackEditorG5";
            this.trackEditorG5.NoteSnapG5 = true;
            this.trackEditorG5.NoteSnapG6 = true;
            this.trackEditorG5.PlaybackPosition = 0D;
            this.trackEditorG5.ScrollToSelectionEnabled = false;
            this.trackEditorG5.SelectCurrentPoint = new System.Drawing.Point(0, 0);
            this.trackEditorG5.SelectStartPoint = new System.Drawing.Point(0, 0);
            this.trackEditorG5.Show108Events = false;
            this.trackEditorG5.ShowNotesGrid = false;
            this.trackEditorG5.Size = new System.Drawing.Size(1101, 117);
            this.trackEditorG5.TabIndex = 8;
            this.trackEditorG5.ViewLyrics = false;
            
            this.trackEditorG5.DragDrop += new System.Windows.Forms.DragEventHandler(this.trackEditorG5_DragDrop);
            this.trackEditorG5.DragEnter += new System.Windows.Forms.DragEventHandler(this.trackEditorG5_DragEnter);
            this.trackEditorG5.Enter += new System.EventHandler(this.trackEditorG5_Enter);
            this.trackEditorG5.Leave += new System.EventHandler(this.trackEditorG5_Leave);
            this.trackEditorG5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackEditorG5_MouseMove);
            // 
            // panelTrackEditorG5TitleBar
            // 
            this.panelTrackEditorG5TitleBar.BackColor = System.Drawing.Color.AliceBlue;
            this.panelTrackEditorG5TitleBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTrackEditorG5TitleBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTrackEditorG5TitleBar.Controls.Add(this.buttonMinimizeG5);
            this.panelTrackEditorG5TitleBar.Controls.Add(this.buttonMaximizeG5);
            this.panelTrackEditorG5TitleBar.Controls.Add(this.labelStatusIconEditor5);
            this.panelTrackEditorG5TitleBar.Controls.Add(this.labelCurrentLoadedG5);
            this.panelTrackEditorG5TitleBar.Controls.Add(this.buttonCloseG5Track);
            this.panelTrackEditorG5TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTrackEditorG5TitleBar.Location = new System.Drawing.Point(0, 0);
            this.panelTrackEditorG5TitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelTrackEditorG5TitleBar.Name = "panelTrackEditorG5TitleBar";
            this.panelTrackEditorG5TitleBar.Size = new System.Drawing.Size(1101, 20);
            this.panelTrackEditorG5TitleBar.TabIndex = 11;
            // 
            // buttonMinimizeG5
            // 
            this.buttonMinimizeG5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMinimizeG5.FlatAppearance.BorderSize = 0;
            this.buttonMinimizeG5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonMinimizeG5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinimizeG5.Location = new System.Drawing.Point(1035, -3);
            this.buttonMinimizeG5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonMinimizeG5.Name = "buttonMinimizeG5";
            this.buttonMinimizeG5.Size = new System.Drawing.Size(22, 22);
            this.buttonMinimizeG5.TabIndex = 17;
            this.buttonMinimizeG5.Text = "-";
            this.buttonMinimizeG5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonMinimizeG5.UseVisualStyleBackColor = true;
            this.buttonMinimizeG5.Click += new System.EventHandler(this.buttonMinimizeG5_Click);
            // 
            // buttonMaximizeG5
            // 
            this.buttonMaximizeG5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMaximizeG5.FlatAppearance.BorderSize = 0;
            this.buttonMaximizeG5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonMaximizeG5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMaximizeG5.Location = new System.Drawing.Point(1056, -3);
            this.buttonMaximizeG5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonMaximizeG5.Name = "buttonMaximizeG5";
            this.buttonMaximizeG5.Size = new System.Drawing.Size(22, 22);
            this.buttonMaximizeG5.TabIndex = 16;
            this.buttonMaximizeG5.Text = "+";
            this.buttonMaximizeG5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonMaximizeG5.UseVisualStyleBackColor = true;
            this.buttonMaximizeG5.Click += new System.EventHandler(this.buttonMaximizeG5_Click);
            // 
            // labelStatusIconEditor5
            // 
            this.labelStatusIconEditor5.AutoSize = true;
            this.labelStatusIconEditor5.ImageKey = "music--exclamation.png";
            this.labelStatusIconEditor5.ImageList = this.imageList1;
            this.labelStatusIconEditor5.Location = new System.Drawing.Point(-1, 0);
            this.labelStatusIconEditor5.Name = "labelStatusIconEditor5";
            this.labelStatusIconEditor5.Size = new System.Drawing.Size(31, 15);
            this.labelStatusIconEditor5.TabIndex = 11;
            this.labelStatusIconEditor5.Text = "        ";
            // 
            // labelCurrentLoadedG5
            // 
            this.labelCurrentLoadedG5.AutoSize = true;
            this.labelCurrentLoadedG5.BackColor = System.Drawing.Color.Transparent;
            this.labelCurrentLoadedG5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentLoadedG5.ForeColor = System.Drawing.Color.Black;
            this.labelCurrentLoadedG5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelCurrentLoadedG5.ImageKey = "(none)";
            this.labelCurrentLoadedG5.ImageList = this.imageList1;
            this.labelCurrentLoadedG5.Location = new System.Drawing.Point(30, 2);
            this.labelCurrentLoadedG5.Name = "labelCurrentLoadedG5";
            this.labelCurrentLoadedG5.Size = new System.Drawing.Size(200, 14);
            this.labelCurrentLoadedG5.TabIndex = 10;
            this.labelCurrentLoadedG5.Text = "No 5 Button Guitar File Loaded";
            this.labelCurrentLoadedG5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelProEditor
            // 
            this.panelProEditor.BackColor = System.Drawing.Color.Transparent;
            this.panelProEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProEditor.Controls.Add(this.trackEditorG6);
            this.panelProEditor.Controls.Add(this.panelTrackEditorG6TitleBar);
            this.panelProEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProEditor.Location = new System.Drawing.Point(0, 163);
            this.panelProEditor.Margin = new System.Windows.Forms.Padding(0);
            this.panelProEditor.Name = "panelProEditor";
            this.panelProEditor.Size = new System.Drawing.Size(1103, 139);
            this.panelProEditor.TabIndex = 14;
            // 
            // trackEditorG6
            // 
            this.trackEditorG6.AllowDrop = true;
            this.trackEditorG6.CurrentDifficulty = ProUpgradeEditor.Common.GuitarDifficulty.Expert;
            this.trackEditorG6.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackEditorG6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackEditorG6.EditMode = ProUpgradeEditor.Common.TrackEditor.EEditMode.Chords;
            this.trackEditorG6.Editor5 = null;
            this.trackEditorG6.EditorPro = null;
            this.trackEditorG6.EditorType = ProUpgradeEditor.Common.TrackEditor.EEditorType.None;
            this.trackEditorG6.EnableRenderMP3Wave = false;
            this.trackEditorG6.GridScalar = 0.25D;
            this.trackEditorG6.GridSnap = true;
            this.trackEditorG6.InPlayback = false;
            this.trackEditorG6.IsMouseOver = false;
            this.trackEditorG6.IsSelecting = false;
            this.trackEditorG6.LoadedFileName = null;
            this.trackEditorG6.Location = new System.Drawing.Point(0, 20);
            this.trackEditorG6.Margin = new System.Windows.Forms.Padding(0);
            this.trackEditorG6.MidiPlaybackPosition = 0;
            this.trackEditorG6.MP3PlaybackOffset = 0;
            
            this.trackEditorG6.Name = "trackEditorG6";
            this.trackEditorG6.NoteSnapG5 = true;
            this.trackEditorG6.NoteSnapG6 = true;
            this.trackEditorG6.PlaybackPosition = 0D;
            this.trackEditorG6.ScrollToSelectionEnabled = false;
            this.trackEditorG6.SelectCurrentPoint = new System.Drawing.Point(0, 0);
            this.trackEditorG6.SelectStartPoint = new System.Drawing.Point(0, 0);
            this.trackEditorG6.Show108Events = false;
            this.trackEditorG6.ShowNotesGrid = false;
            this.trackEditorG6.Size = new System.Drawing.Size(1101, 117);
            this.trackEditorG6.TabIndex = 9;
            this.trackEditorG6.ViewLyrics = false;
            
            this.trackEditorG6.Load += new System.EventHandler(this.trackEditorG6_Load);
            this.trackEditorG6.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDropEditorPro);
            this.trackEditorG6.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnterEditorPro);
            this.trackEditorG6.Enter += new System.EventHandler(this.trackEditorG6_Enter);
            this.trackEditorG6.Leave += new System.EventHandler(this.trackEditorG6_Leave);
            this.trackEditorG6.MouseHover += new System.EventHandler(this.trackEditorG6_MouseHover);
            this.trackEditorG6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackEditorG6_MouseMove);
            // 
            // panelTrackEditorG6TitleBar
            // 
            this.panelTrackEditorG6TitleBar.BackColor = System.Drawing.Color.AliceBlue;
            this.panelTrackEditorG6TitleBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTrackEditorG6TitleBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTrackEditorG6TitleBar.Controls.Add(this.buttonMinimizeG6);
            this.panelTrackEditorG6TitleBar.Controls.Add(this.buttonMaximizeG6);
            this.panelTrackEditorG6TitleBar.Controls.Add(this.buttonCloseG6Track);
            this.panelTrackEditorG6TitleBar.Controls.Add(this.labelStatusIconEditor6);
            this.panelTrackEditorG6TitleBar.Controls.Add(this.labelCurrentLoadedG6);
            this.panelTrackEditorG6TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTrackEditorG6TitleBar.Location = new System.Drawing.Point(0, 0);
            this.panelTrackEditorG6TitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelTrackEditorG6TitleBar.Name = "panelTrackEditorG6TitleBar";
            this.panelTrackEditorG6TitleBar.Size = new System.Drawing.Size(1101, 20);
            this.panelTrackEditorG6TitleBar.TabIndex = 12;
            // 
            // buttonMinimizeG6
            // 
            this.buttonMinimizeG6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMinimizeG6.FlatAppearance.BorderSize = 0;
            this.buttonMinimizeG6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue;
            this.buttonMinimizeG6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinimizeG6.Location = new System.Drawing.Point(1035, -3);
            this.buttonMinimizeG6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonMinimizeG6.Name = "buttonMinimizeG6";
            this.buttonMinimizeG6.Size = new System.Drawing.Size(22, 22);
            this.buttonMinimizeG6.TabIndex = 15;
            this.buttonMinimizeG6.Text = "-";
            this.buttonMinimizeG6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonMinimizeG6.UseVisualStyleBackColor = true;
            this.buttonMinimizeG6.Click += new System.EventHandler(this.buttonMinimizeG6_Click);
            // 
            // buttonMaximizeG6
            // 
            this.buttonMaximizeG6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMaximizeG6.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.buttonMaximizeG6.FlatAppearance.BorderSize = 0;
            this.buttonMaximizeG6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMaximizeG6.Location = new System.Drawing.Point(1056, -3);
            this.buttonMaximizeG6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonMaximizeG6.Name = "buttonMaximizeG6";
            this.buttonMaximizeG6.Size = new System.Drawing.Size(22, 24);
            this.buttonMaximizeG6.TabIndex = 14;
            this.buttonMaximizeG6.Text = "+";
            this.buttonMaximizeG6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonMaximizeG6.UseVisualStyleBackColor = true;
            this.buttonMaximizeG6.Click += new System.EventHandler(this.buttonMaximizeG6_Click);
            // 
            // labelStatusIconEditor6
            // 
            this.labelStatusIconEditor6.AutoSize = true;
            this.labelStatusIconEditor6.ImageKey = "music--exclamation.png";
            this.labelStatusIconEditor6.ImageList = this.imageList1;
            this.labelStatusIconEditor6.Location = new System.Drawing.Point(-1, 0);
            this.labelStatusIconEditor6.Name = "labelStatusIconEditor6";
            this.labelStatusIconEditor6.Size = new System.Drawing.Size(31, 15);
            this.labelStatusIconEditor6.TabIndex = 12;
            this.labelStatusIconEditor6.Text = "        ";
            // 
            // labelCurrentLoadedG6
            // 
            this.labelCurrentLoadedG6.AutoSize = true;
            this.labelCurrentLoadedG6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentLoadedG6.ForeColor = System.Drawing.Color.Black;
            this.labelCurrentLoadedG6.Location = new System.Drawing.Point(30, 2);
            this.labelCurrentLoadedG6.Name = "labelCurrentLoadedG6";
            this.labelCurrentLoadedG6.Size = new System.Drawing.Size(167, 14);
            this.labelCurrentLoadedG6.TabIndex = 10;
            this.labelCurrentLoadedG6.Text = "No Pro Guitar File Loaded";
            // 
            // animationTimer
            // 
            this.animationTimer.Enabled = true;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusStrip1.BackgroundImage")));
            this.statusStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.statusStrip1.Location = new System.Drawing.Point(0, 852);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1103, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip1.BackgroundImage")));
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 2, 0, 0);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1103, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem1,
            this.importToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openPro17ToolStripMenuItem,
            this.openConfigurationToolStripMenuItem});
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem1.Text = "&Open";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "&Open Guitar 5 Midi";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openPro17ToolStripMenuItem
            // 
            this.openPro17ToolStripMenuItem.Name = "openPro17ToolStripMenuItem";
            this.openPro17ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openPro17ToolStripMenuItem.Text = "Open &Pro Midi";
            this.openPro17ToolStripMenuItem.Click += new System.EventHandler(this.openPro17ToolStripMenuItem_Click);
            // 
            // openConfigurationToolStripMenuItem
            // 
            this.openConfigurationToolStripMenuItem.Name = "openConfigurationToolStripMenuItem";
            this.openConfigurationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openConfigurationToolStripMenuItem.Text = "Open &Configuration";
            this.openConfigurationToolStripMenuItem.Click += new System.EventHandler(this.openConfigurationToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveProToolStripMenuItem,
            this.saveProAsToolStripMenuItem,
            this.saveCONPackageToolStripMenuItem,
            this.saveConfigurationToolStripMenuItem,
            this.saveConfigurationAsToolStripMenuItem,
            this.saveProXMLToolStripMenuItem});
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem1.Text = "&Save";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveToolStripMenuItem.Text = "Save Guitar 5 Midi";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveProToolStripMenuItem
            // 
            this.saveProToolStripMenuItem.Name = "saveProToolStripMenuItem";
            this.saveProToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveProToolStripMenuItem.Text = "&Save Pro Midi";
            this.saveProToolStripMenuItem.Click += new System.EventHandler(this.saveProToolStripMenuItem_Click);
            // 
            // saveProAsToolStripMenuItem
            // 
            this.saveProAsToolStripMenuItem.Name = "saveProAsToolStripMenuItem";
            this.saveProAsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveProAsToolStripMenuItem.Text = "Save Pro Midi As";
            this.saveProAsToolStripMenuItem.Click += new System.EventHandler(this.saveProAsToolStripMenuItem_Click);
            // 
            // saveCONPackageToolStripMenuItem
            // 
            this.saveCONPackageToolStripMenuItem.Name = "saveCONPackageToolStripMenuItem";
            this.saveCONPackageToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveCONPackageToolStripMenuItem.Text = "Save CON Package";
            this.saveCONPackageToolStripMenuItem.Click += new System.EventHandler(this.saveCONPackageToolStripMenuItem_Click);
            // 
            // saveConfigurationToolStripMenuItem
            // 
            this.saveConfigurationToolStripMenuItem.Name = "saveConfigurationToolStripMenuItem";
            this.saveConfigurationToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveConfigurationToolStripMenuItem.Text = "Save Configuration";
            this.saveConfigurationToolStripMenuItem.Click += new System.EventHandler(this.saveConfigurationToolStripMenuItem_Click);
            // 
            // saveConfigurationAsToolStripMenuItem
            // 
            this.saveConfigurationAsToolStripMenuItem.Name = "saveConfigurationAsToolStripMenuItem";
            this.saveConfigurationAsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveConfigurationAsToolStripMenuItem.Text = "Save Configuration As";
            this.saveConfigurationAsToolStripMenuItem.Click += new System.EventHandler(this.saveConfigurationAsToolStripMenuItem_Click);
            // 
            // saveProXMLToolStripMenuItem
            // 
            this.saveProXMLToolStripMenuItem.Name = "saveProXMLToolStripMenuItem";
            this.saveProXMLToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveProXMLToolStripMenuItem.Text = "Save Pro XML";
            this.saveProXMLToolStripMenuItem.Click += new System.EventHandler(this.saveProXMLToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cONPackageToolStripMenuItem,
            this.zipFileToolStripMenuItem,
            this.tabVToolStripMenuItem,
            this.mergeProMidiToolStripMenuItem,
            this.guitarProToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "&Import";
            // 
            // cONPackageToolStripMenuItem
            // 
            this.cONPackageToolStripMenuItem.Name = "cONPackageToolStripMenuItem";
            this.cONPackageToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.cONPackageToolStripMenuItem.Text = "CON Package";
            this.cONPackageToolStripMenuItem.Click += new System.EventHandler(this.cONPackageToolStripMenuItem_Click);
            // 
            // zipFileToolStripMenuItem
            // 
            this.zipFileToolStripMenuItem.Name = "zipFileToolStripMenuItem";
            this.zipFileToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.zipFileToolStripMenuItem.Text = "Zip File";
            this.zipFileToolStripMenuItem.Click += new System.EventHandler(this.zipFileToolStripMenuItem_Click);
            // 
            // tabVToolStripMenuItem
            // 
            this.tabVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.midiExportToolStripMenuItem});
            this.tabVToolStripMenuItem.Name = "tabVToolStripMenuItem";
            this.tabVToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.tabVToolStripMenuItem.Text = "Web Tab Exporter Site";
            // 
            // midiExportToolStripMenuItem
            // 
            this.midiExportToolStripMenuItem.Name = "midiExportToolStripMenuItem";
            this.midiExportToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.midiExportToolStripMenuItem.Text = "Import Midi XML";
            this.midiExportToolStripMenuItem.Click += new System.EventHandler(this.midiExportToolStripMenuItem_Click);
            // 
            // mergeProMidiToolStripMenuItem
            // 
            this.mergeProMidiToolStripMenuItem.Name = "mergeProMidiToolStripMenuItem";
            this.mergeProMidiToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.mergeProMidiToolStripMenuItem.Text = "Merge Pro Midi";
            this.mergeProMidiToolStripMenuItem.Click += new System.EventHandler(this.mergeProMidiToolStripMenuItem_Click);
            // 
            // guitarProToolStripMenuItem
            // 
            this.guitarProToolStripMenuItem.Name = "guitarProToolStripMenuItem";
            this.guitarProToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.guitarProToolStripMenuItem.Text = "Guitar Pro";
            this.guitarProToolStripMenuItem.Click += new System.EventHandler(this.guitarProToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xBoxUSBToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 22);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // xBoxUSBToolStripMenuItem
            // 
            this.xBoxUSBToolStripMenuItem.Name = "xBoxUSBToolStripMenuItem";
            this.xBoxUSBToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.xBoxUSBToolStripMenuItem.Text = "XBox USB";
            this.xBoxUSBToolStripMenuItem.Click += new System.EventHandler(this.xBoxUSBToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1103, 874);
            this.Controls.Add(this.tabContainerMain);
            this.Controls.Add(this.panelProEditor);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel5ButtonEditor);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Ziggys Pro Guitar Editor";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            groupBox24.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            groupBox15.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox14.ResumeLayout(false);
            this.panelTrackEditorPro.ResumeLayout(false);
            this.contextStripMidiTracks.ResumeLayout(false);
            this.tabContainerMain.ResumeLayout(false);
            this.tabSongLibraryUtility.ResumeLayout(false);
            this.groupBox39.ResumeLayout(false);
            this.groupBox39.PerformLayout();
            this.groupBox48.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabSongLibSongProperties.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox46.ResumeLayout(false);
            this.groupBox45.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMP3Volume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidiVolume)).EndInit();
            this.groupBox35.ResumeLayout(false);
            this.groupBox28.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.tabSongLibUtility.ResumeLayout(false);
            this.tabSongLibUtility.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.tabSongLibSongUtility.ResumeLayout(false);
            this.groupBox47.ResumeLayout(false);
            this.groupBox47.PerformLayout();
            this.groupBox49.ResumeLayout(false);
            this.groupBox49.PerformLayout();
            this.groupBoxSongUtilFindInFile.ResumeLayout(false);
            this.groupBoxSongUtilFindInFile.PerformLayout();
            this.tabTrackEditor.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox41.ResumeLayout(false);
            this.groupBox41.PerformLayout();
            this.tabNoteEditor.ResumeLayout(false);
            this.groupBox44.ResumeLayout(false);
            this.groupBox44.PerformLayout();
            this.groupBox42.ResumeLayout(false);
            this.groupBox40.ResumeLayout(false);
            this.groupBox37.ResumeLayout(false);
            this.groupBox37.PerformLayout();
            this.groupBox36.ResumeLayout(false);
            this.groupBox36.PerformLayout();
            this.groupBox34.ResumeLayout(false);
            this.groupBox34.PerformLayout();
            this.groupBox29.ResumeLayout(false);
            this.groupBoxMidiInstrument.ResumeLayout(false);
            this.groupBoxMidiInstrument.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.contextMenuStripChannels.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxStrumMarkers.ResumeLayout(false);
            this.tabModifierEditor.ResumeLayout(false);
            this.groupBoxSingleStringTremelo.ResumeLayout(false);
            this.groupBoxSingleStringTremelo.PerformLayout();
            this.groupBoxMultiStringTremelo.ResumeLayout(false);
            this.groupBoxMultiStringTremelo.PerformLayout();
            this.groupBoxArpeggio.ResumeLayout(false);
            this.groupBoxArpeggio.PerformLayout();
            this.groupBoxPowerup.ResumeLayout(false);
            this.groupBoxPowerup.PerformLayout();
            this.groupBoxSolo.ResumeLayout(false);
            this.groupBoxSolo.PerformLayout();
            this.tabPageEvents.ResumeLayout(false);
            this.groupBox43.ResumeLayout(false);
            this.groupBoxTextEvents.ResumeLayout(false);
            this.groupBoxTextEvents.PerformLayout();
            this.groupBoxProBassTrainers.ResumeLayout(false);
            this.groupBoxProBassTrainers.PerformLayout();
            this.groupBoxProGuitarTrainers.ResumeLayout(false);
            this.groupBoxProGuitarTrainers.PerformLayout();
            this.tabPackageEditor.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.groupBox32.ResumeLayout(false);
            this.groupBox32.PerformLayout();
            this.contextToolStripPackageEditor.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.groupBox30.ResumeLayout(false);
            this.groupBox30.PerformLayout();
            this.tabUSBDrive.ResumeLayout(false);
            this.tabUSBDrive.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.groupBox38.ResumeLayout(false);
            this.groupBox33.ResumeLayout(false);
            this.groupBox33.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.panel5ButtonEditor.ResumeLayout(false);
            this.panelTrackEditorG5TitleBar.ResumeLayout(false);
            this.panelTrackEditorG5TitleBar.PerformLayout();
            this.panelProEditor.ResumeLayout(false);
            this.panelTrackEditorG6TitleBar.ResumeLayout(false);
            this.panelTrackEditorG6TitleBar.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPro17ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProToolStripMenuItem;
        private System.Windows.Forms.TabControl tabContainerMain;
        private System.Windows.Forms.TabPage tabTrackEditor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonInitFromG5;
        private System.Windows.Forms.ToolStripMenuItem saveProAsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabNoteEditor;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelMidiInputDeviceState;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.CheckBox checkKeepSelection;
        private System.Windows.Forms.CheckBox checkBoxClearAfterNote;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox checkRealtimeNotes;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.CheckBox checkThreeNotePowerChord;
        private System.Windows.Forms.CheckBox checkTwoNotePowerChord;
        private System.Windows.Forms.CheckBox checkScrollToSelection;
        private System.Windows.Forms.CheckBox checkChordMode;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxIncludeGuitar22;
        private System.Windows.Forms.CheckBox checkBoxInitSelectedTrackOnly;
        private System.Windows.Forms.TabPage tabModifierEditor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.ListBox listBoxSolos;
        private System.Windows.Forms.TextBox textBox26;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.TextBox textBox27;
        private System.Windows.Forms.TextBox textBox28;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.TextBox textBox29;
        private System.Windows.Forms.TextBox textBox30;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TextBox textBoxZoom;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox checkBoxCreateArpeggioHelperNotes;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.CheckBox checkKBQuickEdit;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox0;
        private System.Windows.Forms.CheckBox checkIsHammeron;
        private System.Windows.Forms.CheckBox checkIsSlide;
        private System.Windows.Forms.CheckBox checkIsTap;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordUpTick;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordDownTick;
        private System.Windows.Forms.CheckBox checkIsX;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox1;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox2;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox3;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox4;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordChannelBox5;
        private System.Windows.Forms.Button buttonAddSlideHammeron;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.Button button33;
        private System.Windows.Forms.Button button36;
        private System.Windows.Forms.Button button38;
        private System.Windows.Forms.Button button37;
        private System.Windows.Forms.GroupBox groupBoxStrumMarkers;
        private System.Windows.Forms.CheckBox checkIsSlideReversed;
        private System.Windows.Forms.Button buttonStrumNone;
        private System.Windows.Forms.CheckBox checkStrumLow;
        private System.Windows.Forms.CheckBox checkStrumMid;
        private System.Windows.Forms.CheckBox checkStrumHigh;
        private System.Windows.Forms.Button button39;
        private System.Windows.Forms.Button button40;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button41;
        private System.Windows.Forms.Button button42;
        private System.Windows.Forms.Button button43;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.Button button44;
        private System.Windows.Forms.Button button45;
        private System.Windows.Forms.Button button46;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button47;
        private System.Windows.Forms.Button button48;
        private System.Windows.Forms.Button button49;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripFileName5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripFileName6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListBox listBoxStoredChords;
        private System.Windows.Forms.Button button50;
        private System.Windows.Forms.Button button51;
        private System.Windows.Forms.Button button52;
        private System.Windows.Forms.Button button53;
        private System.Windows.Forms.Button button54;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textBoxNoteEditorSelectedChordTickLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCreateStatus;
        private System.Windows.Forms.Button button55;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkIndentBString;
        private System.Windows.Forms.Button button58;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioDifficultyExpert;
        private System.Windows.Forms.RadioButton radioDifficultyHard;
        private System.Windows.Forms.RadioButton radioDifficultyMedium;
        private System.Windows.Forms.RadioButton radioDifficultyEasy;
        private System.Windows.Forms.Button button56;
        private System.Windows.Forms.Button button59;

        private System.Windows.Forms.TabPage tabPackageEditor;

        private System.Windows.Forms.Button button62;
        private System.Windows.Forms.TextBox textBoxDefaultCONFileLocation;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button button63;
        private System.Windows.Forms.TextBox textBoxDefaultMidi5FileLocation;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button button64;
        private System.Windows.Forms.TextBox textBoxDefaultMidiProFileLocation;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TabPage tabSongLibraryUtility;
        private System.Windows.Forms.Button buttonSongPropertiesSaveChanges;
        private System.Windows.Forms.Button button66;
        private System.Windows.Forms.Button button65;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox textBoxSongLibProMidiFileName;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ListBox listBoxSongLibrary;
        private System.Windows.Forms.Button button69;
        private System.Windows.Forms.TextBox textBoxSongLibG5MidiFileName;
        private System.Windows.Forms.Button button68;
        private System.Windows.Forms.Button button71;
        private System.Windows.Forms.TextBox textBoxSongLibConFile;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.CheckBox checkUseDefaultFolders;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox checkBoxSongLibIsComplete;
        private System.Windows.Forms.CheckBox checkBoxSongLibCopyGuitar;
        private System.Windows.Forms.CheckBox checkBoxSongLibHasGuitar;
        private System.Windows.Forms.CheckBox checkBoxSongLibHasBass;

        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TreeView treePackageContents;
        private System.Windows.Forms.Button button60;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBoxPackageDTAText;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkGenDiffCopyGuitarToBass;
        private System.Windows.Forms.CheckBox checkBoxSongLibIsFinalized;
        private System.Windows.Forms.ProgressBar progressBarGenCompletedDifficulty;
        private System.Windows.Forms.Button button75;
        private System.Windows.Forms.TextBox textBoxCopyAllCONFolder;
        private System.Windows.Forms.Button button74;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabSongLibSongProperties;
        private System.Windows.Forms.TabPage tabSongLibUtility;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox checkBoxFirstMatchOnly;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.CheckBox checkBoxMatchLengths;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button30;
        private System.Windows.Forms.Button button80;
        private System.Windows.Forms.Button button79;
        private System.Windows.Forms.Button button78;
        private System.Windows.Forms.Button button81;
        private System.Windows.Forms.TextBox textBoxSongLibBatchResults;
        private System.Windows.Forms.CheckBox checkBoxSmokeTest;
        private System.Windows.Forms.Button buttonSongLibCancel;
        private System.Windows.Forms.CheckBox checkBoxSkipGenIfEasyNotes;
        private System.Windows.Forms.CheckBox checkBoxMultiSelectionSongList;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button button77;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button button84;
        private System.Windows.Forms.Button button83;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Button buttonAddHammeron;
        private System.Windows.Forms.Button button85;
        private System.Windows.Forms.Button button87;
        private System.Windows.Forms.CheckBox checkBoxSearchByNoteType;
        private System.Windows.Forms.CheckBox checkBoxSearchByNoteFret;
        private System.Windows.Forms.CheckBox checkBoxSearchByNoteStrum;
        private System.Windows.Forms.Button button88;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.CheckBox checkBoxShowMidiChannelEdit;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBoxScrollToSelectionOffset;
        private System.Windows.Forms.RadioButton radioGrid32Note;
        private System.Windows.Forms.RadioButton radioGrid64thNote;
        private System.Windows.Forms.RadioButton radioGridQuarterNote;
        private System.Windows.Forms.RadioButton radioGrid8Note;
        private System.Windows.Forms.RadioButton radioGrid16Note;
        private System.Windows.Forms.CheckBox checkViewNotesGridPro;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.CheckBox checkViewNotesGrid5Button;
        private System.Windows.Forms.GroupBox groupBoxSingleStringTremelo;
        private System.Windows.Forms.GroupBox groupBoxMultiStringTremelo;
        private System.Windows.Forms.GroupBox groupBoxArpeggio;
        private System.Windows.Forms.GroupBox groupBoxPowerup;
        private System.Windows.Forms.GroupBox groupBoxSolo;
        private System.Windows.Forms.Button button89;
        private System.Windows.Forms.CheckBox checkBoxBatchRebuildCON;
        private System.Windows.Forms.CheckBox checkBoxBatchGuitarBassCopy;
        private System.Windows.Forms.CheckBox checkBoxBatchGenerateDifficulties;
        private System.Windows.Forms.CheckBox checkBoxSetBassToGuitarDifficulty;
        private System.Windows.Forms.GroupBox groupBoxMidiInstrument;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxMinimumNoteWidth;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.CheckBox checkBoxBatchProcessFinalized;
        private System.Windows.Forms.CheckBox checkBoxBatchProcessIncomplete;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.CheckBox checkBoxInitSelectedDifficultyOnly;
        private System.Windows.Forms.Button button90;
        private System.Windows.Forms.Button button73;
        private System.Windows.Forms.TextBox textBoxCopyAllG5MidiFolder;
        private System.Windows.Forms.Button button82;
        private System.Windows.Forms.TextBox textBoxCopyAllProFolder;
        private System.Windows.Forms.Button button72;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.Button button91;
        private System.Windows.Forms.Button button92;
        private System.Windows.Forms.CheckBox checkBoxBatchCheckCON;
        private System.Windows.Forms.Label labelCurrentLoadedG5;
        private System.Windows.Forms.Panel panelTrackEditorG5TitleBar;
        private System.Windows.Forms.Panel panelTrackEditorG6TitleBar;
        private System.Windows.Forms.Label labelCurrentLoadedG6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.Button button93;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.TextBox textBox43;
        private System.Windows.Forms.TextBox textBox44;
        private System.Windows.Forms.TextBox textBox45;
        private System.Windows.Forms.TextBox textBox46;
        private System.Windows.Forms.TextBox textBox47;
        private System.Windows.Forms.TextBox textBox48;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.TextBox textBox42;
        private System.Windows.Forms.TextBox textBox41;
        private System.Windows.Forms.TextBox textBox40;
        private System.Windows.Forms.TextBox textBox39;
        private System.Windows.Forms.TextBox textBox38;
        private System.Windows.Forms.TextBox textBox37;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.Button button97;
        private System.Windows.Forms.Button button98;
        private System.Windows.Forms.Button buttonPlayHoldBoxMidi;
        private System.Windows.Forms.Button button57;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox textBoxCONShortName;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox textBoxCONSongID;
        private System.Windows.Forms.ComboBox comboProGDifficulty;
        private System.Windows.Forms.ComboBox comboProBDifficulty;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.TextBox textBox49;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox30;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.GroupBox groupBox32;
        private System.Windows.Forms.Button button61;
        private System.Windows.Forms.GroupBox groupBox33;
        private System.Windows.Forms.Button button70;
        private System.Windows.Forms.ComboBox comboMidiInstrument;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonRebuildPackage;
        private System.Windows.Forms.ToolStripMenuItem saveCONPackageToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboMidiDevice;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Timer timerMidiPlayback;
        private System.Windows.Forms.CheckBox checkBoxMidiPlaybackScroll;
        private System.Windows.Forms.Button buttonSongPropertiesCheckPackage;
        private System.Windows.Forms.Button buttonPackageEditorOpenPackage;
        public System.Windows.Forms.CheckBox checkBoxGridSnap;
        private System.Windows.Forms.Button buttonCreateSongFromOpenMidi;
        private System.Windows.Forms.Button button106;
        private System.Windows.Forms.Button button105;
        private System.Windows.Forms.Button buttonExecuteBatchGuitarBassCopy;
        private System.Windows.Forms.Button button103;
        private System.Windows.Forms.Button buttonSongLibViewPackageContents;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button108;
        private System.Windows.Forms.Button button109;
        private System.Windows.Forms.TextBox textBoxPlaceNoteFret;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox checkBoxUseCurrentChord;
        private System.Windows.Forms.CheckBox checkBoxAllowOverwriteChord;
        private System.Windows.Forms.RadioButton radioGrid128thNote;
        private System.Windows.Forms.Button button110;
        private System.Windows.Forms.CheckBox checkBoxLoadLastSongStartup;
        private System.Windows.Forms.TextBox textBoxMidiScrollFreq;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xBoxUSBToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox35;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;

        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.GroupBox groupBox36;
        private System.Windows.Forms.TextBox textClearHoldBox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox checkBoxEnableMidiInput;
        private System.Windows.Forms.GroupBox groupBox37;
        private System.Windows.Forms.CheckBox checkBoxEnableClearTimer;
        private System.Windows.Forms.CheckBox checkBoxPlayMidiStrum;
        private System.Windows.Forms.CheckBox checkBoxClearIfNoFrets;
        private System.Windows.Forms.CheckBox checkBoxChordStrum;
        private System.Windows.Forms.CheckBox checkBoxMidiInputStartup;
        private System.Windows.Forms.GroupBox groupBox38;
        private System.Windows.Forms.Button button111;
        private System.Windows.Forms.ComboBox comboBoxMidiInput;
        private System.Windows.Forms.Button button112;
        private System.Windows.Forms.Button button113;
        private System.Windows.Forms.Button button114;
        private System.Windows.Forms.Button button115;
        private System.Windows.Forms.CheckBox checkView5Button;
        private System.Windows.Forms.Panel panel5ButtonEditor;
        private System.Windows.Forms.Panel panelProEditor;
        private System.Windows.Forms.CheckBox checkBoxMatchAllFrets;
        private System.Windows.Forms.Button button116;
        private System.Windows.Forms.Button button118;
        private System.Windows.Forms.TextBox textBoxCompressAllZipFile;
        private System.Windows.Forms.Button button117;
        private System.Windows.Forms.CheckBox checkBoxCompressAllInDefaultCONFolder;
        private System.Windows.Forms.CheckBox checkBoxMatchLength6;
        private System.Windows.Forms.CheckBox checkBoxMatchSpacing;
        private System.Windows.Forms.CheckBox checkBoxRenderMouseSnap;
        public System.Windows.Forms.CheckBox checkBoxSnapToCloseNotes;
        private System.Windows.Forms.Button button119;
        private System.Windows.Forms.TextBox textBoxNoteSnapDistance;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBoxGridSnapDistance;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBoxNoteCloseDist;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.CheckBox checkBoxKeepLengths;
        private System.Windows.Forms.ToolStripMenuItem saveConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigurationAsToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkMatchBeat;
        private System.Windows.Forms.Button button120;
        private System.Windows.Forms.Button button121;
        private System.Windows.Forms.CheckBox checkBoxMatchForwardOnly;
        private System.Windows.Forms.Button button122;
        private System.Windows.Forms.Button button123;
        private System.Windows.Forms.Button button124;
        private System.Windows.Forms.Button buttonCopyAllConToUSB;
        private System.Windows.Forms.TabPage tabUSBDrive;
        private System.Windows.Forms.Button buttonUSBSetDefaultFolder;
        private System.Windows.Forms.Button buttonUSBRenameFolder;
        private System.Windows.Forms.Button buttonUSBCreateFolder;
        private System.Windows.Forms.TextBox textBoxUSBFolder;
        private System.Windows.Forms.Button buttonUSBRefresh;
        private System.Windows.Forms.Button buttonUSBCopySelectedSongToUSB;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button buttonUSBExtractSelectedFiles;
        private System.Windows.Forms.Button buttonUSBExtractFolder;
        private System.Windows.Forms.Button buttonUSBDeleteSelected;
        private System.Windows.Forms.ComboBox comboUSBList;
        private System.Windows.Forms.Button button125;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TreeView treeUSBContents;
        private System.Windows.Forms.Button buttonUSBRestoreImage;
        private System.Windows.Forms.Button buttonUSBCreateImage;
        private System.Windows.Forms.Button buttonUSBDeleteFile;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button buttonUSBAddFile;
        private System.Windows.Forms.Button buttonUSBSelectCompletedSongs;
        private System.Windows.Forms.Button buttonUSBSelectAllSongs;
        private System.Windows.Forms.ListView listUSBFileView;
        private System.Windows.Forms.ProgressBar progressUSBSongs;
        private System.Windows.Forms.Button buttonUSBAddFolder;
        private System.Windows.Forms.Button button126;
        private System.Windows.Forms.Button buttonExecuteBatchCopyUSB;
        private System.Windows.Forms.CheckBox checkBoxBatchCopyUSB;
        private System.Windows.Forms.Button buttonUSBViewPackage;
        private System.Windows.Forms.Button buttonUSBRenameFile;
        private System.Windows.Forms.TextBox textBoxUSBFileName;
        private System.Windows.Forms.ProgressBar progressUSBFiles;
        private System.Windows.Forms.Button buttonBatchOpenResult;
        private System.Windows.Forms.CheckBox checkBatchOpenWhenCompleted;
        private ProUpgradeEditor.Common.TrackEditor trackEditorG6;
        private ProUpgradeEditor.Common.TrackEditor trackEditorG5;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listBoxUSBSongs;
        private System.Windows.Forms.ContextMenuStrip contextStripMidiTracks;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Button button67;
        private System.Windows.Forms.GroupBox groupBox39;
        private System.Windows.Forms.Label labelStatusIconEditor5;
        private System.Windows.Forms.Label labelStatusIconEditor6;
        private System.Windows.Forms.Panel panelTrackEditorPro;
        private EditorResources.Components.PEMidiTrackEditPanel midiTrackEditorPro;
        private System.Windows.Forms.Panel panel8;
        private EditorResources.Components.PEMidiTrackEditPanel midiTrackEditorG5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox40;
        private System.Windows.Forms.RadioButton radioNoteEditDifficultyExpert;
        private System.Windows.Forms.RadioButton radioNoteEditDifficultyHard;
        private System.Windows.Forms.RadioButton radioNoteEditDifficultyMedium;
        private System.Windows.Forms.RadioButton radioNoteEditDifficultyEasy;
        private System.Windows.Forms.Button buttonUSBCheckFile;
        private System.Windows.Forms.Button buttonCloseG5Track;
        private System.Windows.Forms.Button buttonCloseG6Track;
        private System.Windows.Forms.Button button9;
        public System.Windows.Forms.CheckBox checkSnapToCloseG5;
        private System.Windows.Forms.Button button76;
        private System.Windows.Forms.Button button128;
        private System.Windows.Forms.Button buttonSongLibCopyPackageToUSB;
        private System.Windows.Forms.CheckBox checkBoxPackageEditExtractDTAMidOnly;
        private System.Windows.Forms.GroupBox groupBox41;
        private System.Windows.Forms.TextBox textBoxTempoDenominator;
        private System.Windows.Forms.TextBox textBoxTempoNumerator;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button130;
        private System.Windows.Forms.Button buttonInitTempo;
        private System.Windows.Forms.Button button131;
        private System.Windows.Forms.Button buttonDownString;
        private System.Windows.Forms.Button buttonUp12;
        private System.Windows.Forms.GroupBox groupBox42;
        private System.Windows.Forms.Button button99;
        private System.Windows.Forms.Button button100;
        private System.Windows.Forms.Button buttonNoteEditorCopyPatternPresetUpdate;
        private System.Windows.Forms.Button buttonNoteEditorCopyPatternPresetRemove;
        private System.Windows.Forms.Button buttonNoteEditorCopyPatternPresetCreate;
        private System.Windows.Forms.ComboBox comboNoteEditorCopyPatternPreset;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.CheckBox checkBoxAutoSelectNext;
        private System.Windows.Forms.Button buttonDeleteSelectedNotes;
        private System.Windows.Forms.CheckBox checkBoxMatchRemoveExisting;
        private System.Windows.Forms.Button buttonReplaceStoredChordWithCopyPattern;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripChannels;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.Button buttonCheckAllInFolder;
        private System.Windows.Forms.Button button132;
        private System.Windows.Forms.Button buttonUtilMethodSnapToG5;
        private System.Windows.Forms.Button button134;
        private System.Windows.Forms.TabPage tabPageEvents;
        private System.Windows.Forms.GroupBox groupBoxProGuitarTrainers;
        private System.Windows.Forms.ListBox listProGuitarTrainers;
        private System.Windows.Forms.Button button135;
        private System.Windows.Forms.Button buttonRemoveProGuitarTrainer;
        private System.Windows.Forms.Button buttonAddProGuitarTrainer;
        private System.Windows.Forms.Label labelProGuitarTrainerStatus;
        private System.Windows.Forms.TextBox textBoxProGuitarTrainerBeginTick;
        private System.Windows.Forms.TextBox textBoxProGuitarTrainerEndTick;
        private System.Windows.Forms.Button buttonUpdateProGuitarTrainer;
        private System.Windows.Forms.Button buttonCancelProGuitarTrainer;
        private System.Windows.Forms.GroupBox groupBoxProBassTrainers;
        private System.Windows.Forms.ListBox listProBassTrainers;
        private System.Windows.Forms.Button buttonRefreshProBassTrainer;
        private System.Windows.Forms.Button buttonRemoveProBassTrainer;
        private System.Windows.Forms.Button buttonCreateProBassTrainer;
        private System.Windows.Forms.Label labelProBassTrainerStatus;
        private System.Windows.Forms.TextBox textBoxProBassTrainerBeginTick;
        private System.Windows.Forms.TextBox textBoxProBassTrainerEndTick;
        private System.Windows.Forms.Button buttonUpdateProBassTrainer;
        private System.Windows.Forms.Button buttonCancelProBassTrainer;
        private System.Windows.Forms.GroupBox groupBoxTextEvents;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listTextEvents;
        private System.Windows.Forms.Button buttonRefreshTextEvents;
        private System.Windows.Forms.Button buttonDeleteTextEvent;
        private System.Windows.Forms.TextBox textBoxEventTick;
        private System.Windows.Forms.TextBox textBoxEventText;
        private System.Windows.Forms.Button buttonUpdateTextEvent;
        private System.Windows.Forms.CheckBox checkTrainerLoopableProBass;
        private System.Windows.Forms.CheckBox checkTrainerLoopableProGuitar;
        private System.Windows.Forms.CheckBox checkBoxShowTrainersInTextEvents;
        private System.Windows.Forms.Button button136;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.CheckBox checkBatchCopyTextEvents;
        private System.Windows.Forms.Button buttonBatchBuildTextEvents;
        private System.Windows.Forms.CheckBox checkBatchGenerateTrainersIfNone;
        private System.Windows.Forms.GroupBox groupBox43;
        private System.Windows.Forms.Button buttonRefresh108Events;
        private System.Windows.Forms.CheckBox checkBoxShow108;
        private System.Windows.Forms.ListBox comboBox180;
        private System.Windows.Forms.GroupBox groupBox44;
        private System.Windows.Forms.CheckBox checkChordNameEb;
        private System.Windows.Forms.CheckBox checkChordNameD;
        private System.Windows.Forms.CheckBox checkChordNameDb;
        private System.Windows.Forms.CheckBox checkChordNameC;
        private System.Windows.Forms.CheckBox checkChordNameG;
        private System.Windows.Forms.CheckBox checkChordNameGb;
        private System.Windows.Forms.CheckBox checkChordNameB;
        private System.Windows.Forms.CheckBox checkChordNameBb;
        private System.Windows.Forms.CheckBox checkChordNameA;
        private System.Windows.Forms.CheckBox checkChordNameAb;
        private System.Windows.Forms.CheckBox checkChordNameF;
        private System.Windows.Forms.CheckBox checkChordNameE;
        private System.Windows.Forms.CheckBox checkChordNameSlash;
        private System.Windows.Forms.CheckBox checkChordNameHide;
        private System.Windows.Forms.Button buttonDownOctave;
        private System.Windows.Forms.Button buttonUpOctave;
        private System.Windows.Forms.Button buttonUtilMethodFindNoteLenZero;
        private System.Windows.Forms.Button buttonUtilMethodSetToG5;
        private System.Windows.Forms.Button buttonShortToG5Len;
        private System.Windows.Forms.Button buttonSongPropertiesExploreMP3Location;
        private System.Windows.Forms.Button buttonSongPropertiesChooseMP3Location;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox textBoxSongPropertiesMP3Location;
        private System.Windows.Forms.Button buttonSongPropertiesMidiPause;
        private System.Windows.Forms.Button buttonSongPropertiesMidiPlay;
        private System.Windows.Forms.TextBox textBoxSongPropertiesMP3StartOffset;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.CheckBox checkBoxSongPropertiesEnableMP3Playback;
        private System.Windows.Forms.CheckBox checkBoxEnableMidiPlayback;
        private System.Windows.Forms.TrackBar trackBarMP3Volume;
        private System.Windows.Forms.TrackBar trackBarMidiVolume;
        private System.Windows.Forms.GroupBox groupBox46;
        private System.Windows.Forms.CheckBox checkBoxAutoGenGuitarEasy;
        private System.Windows.Forms.CheckBox checkBoxAutoGenGuitarMedium;
        private System.Windows.Forms.CheckBox checkBoxAutoGenGuitarHard;
        private System.Windows.Forms.GroupBox groupBox45;
        private System.Windows.Forms.CheckBox checkBoxAutoGenBassEasy;
        private System.Windows.Forms.CheckBox checkBoxAutoGenBassMedium;
        private System.Windows.Forms.CheckBox checkBoxAutoGenBassHard;
        private System.Windows.Forms.Button buttonFindMP3Offset;
        private System.Windows.Forms.Timer animationTimer;
        private System.Windows.Forms.Button buttonMinimizeG5;
        private System.Windows.Forms.Button buttonMaximizeG5;
        private System.Windows.Forms.Button buttonMinimizeG6;
        private System.Windows.Forms.Button buttonMaximizeG6;
        private System.Windows.Forms.Button button94;
        private System.Windows.Forms.TextBox textBoxBatchUtilExtractXML;
        private System.Windows.Forms.Button button95;
        private System.Windows.Forms.CheckBox checkBoxBatchUtilExtractXMLPro;
        private System.Windows.Forms.CheckBox checkBoxBatchUtilExtractXMLG5;
        private System.Windows.Forms.Button buttonSongPropertiesViewMp3Preview;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cONPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zipFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabPage tabSongLibSongUtility;
        private System.Windows.Forms.GroupBox groupBox47;
        private System.Windows.Forms.Button buttonSongUtilSearchFolderExplore;
        private System.Windows.Forms.TextBox textBoxSongUtilSearchFolder;
        private System.Windows.Forms.Button buttonSongUtilSearchForG5FromOpenPro;
        private System.Windows.Forms.Button buttonNoteUtilSelectAll;
        private System.Windows.Forms.Button buttonSongLibListFilterReset;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox textBoxSongLibListFilter;
        private System.Windows.Forms.GroupBox groupBox48;
        private System.Windows.Forms.CheckBox checkBoxSongLibSongListSortAscending;
        private System.Windows.Forms.RadioButton radioSongLibSongListSortCompleted;
        private System.Windows.Forms.RadioButton radioSongLibSongListSortID;
        private System.Windows.Forms.RadioButton radioSongLibSongListSortName;
        private System.Windows.Forms.ToolStripMenuItem saveProXMLToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioGridHalfNote;
        private System.Windows.Forms.RadioButton radioGridWholeNote;
        private System.Windows.Forms.ToolStripMenuItem tabVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem midiExportToolStripMenuItem;
        private System.Windows.Forms.Button buttonFixOverlappingNotes;
        private System.Windows.Forms.Button buttonSelectForward;
        private System.Windows.Forms.Button buttonSelectBack;
        private System.Windows.Forms.ToolStripMenuItem mergeProMidiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guitarProToolStripMenuItem;
        private System.Windows.Forms.Button buttonPackageViewerSave;
        private System.Windows.Forms.ContextMenuStrip contextToolStripPackageEditor;
        private System.Windows.Forms.ToolStripMenuItem toolStripPackageEditorDeleteFile;
        private System.Windows.Forms.Button buttonAddTextEvent;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox textBoxSongUtilFindInFileData1;
        private System.Windows.Forms.GroupBox groupBoxSongUtilFindInFile;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TextBox textBoxSongUtilFindInFileChan;
        private System.Windows.Forms.Button buttonSongUtilFindInFileSearch;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox textBoxSongUtilFindInFileText;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox textBoxSongUtilFindInFileData2;
        private System.Windows.Forms.GroupBox groupBox49;
        private System.Windows.Forms.Button buttonSongUtilFindInFileResultsOpenWindow;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInFileResultsOpenCompleted;
        private System.Windows.Forms.TextBox textBoxSongUtilFindInFileResults;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInFileMatchCountOnly;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInFileFirstMatchOnly;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInFileSelectedSongOnly;
        private System.Windows.Forms.Button buttonSongUtilFindInFileDistinctText;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInFileMatchWholeWord;
        private System.Windows.Forms.Button buttonSongToolSnapNotes;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Button buttonSongUtilFindFolder;
        private System.Windows.Forms.TextBox textBoxSongUtilFindFolder;
        private System.Windows.Forms.CheckBox checkBoxSongUtilFindInProOnly;
        private System.Windows.Forms.ComboBox comboBoxNoteEditorChordName;
        private System.Windows.Forms.Label label61;


    }
}

