import React from 'react';
import { useDrag, DragSourceMonitor, useDrop } from 'react-dnd'

const styleBox: React.CSSProperties = {
    border: '1px dashed gray',
    backgroundColor: 'white',
    padding: '0.5rem',
    marginRight: '1rem',
    marginBottom: '0.5rem',
    cursor: 'move',
    float: 'left',
  }
  
  interface BoxProps {
    name: string
  }
  
  export const DropBox: React.FC<{
    type: string,
    field: any,
    addField: Function,
    readOnly: boolean
  }> = ({ field, addField, readOnly, type }) => {

    const [{ isDragging }, drag] = useDrag({
      item: { name: field.Name, type },
  
      end: (item: { name: string } | undefined, monitor: DragSourceMonitor) => {
        const dropResult = monitor.getDropResult()
        if (item && dropResult && !dropResult.fieldId) {
          addField(field, dropResult.name);
        }
      },
      
      collect: (monitor) => ({
        isDragging: monitor.isDragging(),
      }),

      canDrag: !readOnly
    })
    const opacity = isDragging || readOnly ? 0.4 : 1
  
    return (
      <div ref={drag} style={{ ...styleBox, opacity }}>
            {field.Text}
      </div>
    )
  }

const style: React.CSSProperties = {
    minHeight: '2.5rem',
    width: '15rem',
    margin: '0.5rem',
    float: 'left',
    border: "1px dashed gray"
  }
  
  export const FieldPanel: React.FC<any> = ({panelType, Id, FieldId, children}) => {
    const [{ canDrop, isOver }, drop] = useDrop({
      accept: panelType,
      drop: () => ({ name: Id, fieldId: FieldId }),
      collect: (monitor) => ({
        isOver: monitor.isOver(),
        canDrop: monitor.canDrop(),
      }),
    })
  
    const isActive = canDrop && isOver
    let backgroundColor = '#fff'
    if (isActive) {
      backgroundColor = 'darkgreen'
    } else if (canDrop) {
      backgroundColor = 'darkkhaki'
    }
  
    return (<div ref={drop} style={{ ...style, backgroundColor, width: panelType === "BUTTONBOX"? "6rem" : "15rem" }}>{children}</div>);
  }

  export const MoveBox: React.FC<{
    item: any,
    moveField: Function,
    children: any
  }> = ({ item, moveField, children }) => {
    
    const [{ isDragging }, drag] = useDrag({
      item: { name: item.id.toString(), type: "FIELDBOX" },
      
      end: (item: { name: string } | undefined, monitor: DragSourceMonitor) => {
        const dropResult = monitor.getDropResult();
        if (item && dropResult && !dropResult.fieldId) {
          moveField(item.name, dropResult.name);
        }
      },
      
      collect: (monitor) => ({
        isDragging: monitor.isDragging(),
      }),
    })
    const opacity = isDragging ? 0.4 : 1
  
    return (
      <div ref={drag} style={{ opacity }}>
        {children}
      </div>
    )
  }