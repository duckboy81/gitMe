Imports System.Drawing

Public Class PlateCaree
    Public Function Project(geographicCoordinate As PointF) As PointF
        'First, convert the geographic coordinate to radians
        Dim radianX As Double = geographicCoordinate.X * (Math.PI / 180)
        Dim radianY As Double = geographicCoordinate.Y * (Math.PI / 180)

        'Make a new Point object
        Dim result As PointF = New PointF()

        'Calculate the projected X coordinate
        result.X = (radianX * Math.Cos(0))

        'Calculate the projected Y coordinate
        result.Y = radianY

        'Return the result
        Return result
    End Function

    Public Function Deproject(projectedCoordinate As PointF) As PointF
        'Make a new point to store the result
        Dim result As PointF = New PointF()

        'Calculate the geographic X coordinate (longitude)
        result.X = (projectedCoordinate.X / Math.Cos(0) / (Math.PI / 180.0))

        'Calculate the geographic Y coordinate (latitude)
        result.Y = (projectedCoordinate.Y / (Math.PI / 180.0))

        Return result
    End Function
End Class