using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace RevitAPITrainingFurniture
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private readonly XYZ insertionPoint;
        private readonly FamilySymbol SelectedFurnitureType;

        public List<FamilySymbol> FurnitureTypes { get; }
        public List<FamilySymbol> FamilyTypes { get; } = new List<FamilySymbol>();

        public List<Level> Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; }
        public object InsertionPoint { get; }
        public List<XYZ> Points { get; } = new List<XYZ>();

        public Level SelectedLevel { get; set; }

        public FamilySymbol SelectedFamilyType { get; set; }
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            FurnitureTypes = FurnitureUtils.GetFurnitureTypes(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            insertionPoint = SelectionUtils.GetPoint(commandData, "Укажите точку");
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, insertionPoint, SelectedLevel);

            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}