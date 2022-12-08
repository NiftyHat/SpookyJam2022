using UnityEngine;
using UnityEngine.UI;

namespace NiftyFramework
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
             Uniform,
             Width,
             Height
        }
        
        
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;
        public FitType fitType;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            float sqrRoot = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRoot);
            columns = Mathf.CeilToInt(sqrRoot);

            if (fitType == FitType.Width)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            if (fitType == FitType.Height)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellMaxWidth = parentWidth / columns;
            float cellMaxHeight = parentHeight / rows;
            
            float cellRowSpacing = spacing.x / columns;
            float cellColSpacing = spacing.x / rows;

            float cellRowPadding = (padding.right / columns) - (padding.left / columns);
            float cellColPadding = (padding.top / rows) - (padding.bottom / rows);

            float cellWidth = cellMaxWidth - cellRowSpacing - cellRowPadding;
            float cellHeight = cellMaxHeight - cellColSpacing - cellColPadding;
            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) +  padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) +padding.top;
                
                
                SetChildAlongAxis(item, 0 ,xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            
        }

        public override void SetLayoutHorizontal()
        {
            
        }

        public override void SetLayoutVertical()
        {

        }
    }
}