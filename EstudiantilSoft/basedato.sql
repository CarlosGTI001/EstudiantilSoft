CREATE TRIGGER InsertEstudiantePago ON Estudiantes AFTER INSERT
AS
BEGIN
	DECLARE @EstudianteID as INT
	SET @EstudianteID = (SELECT EstudianteID FROM inserted)
	INSERT INTO pagos VALUES (@EstudianteID, CAST( GETDATE() AS Date ), 1500, 'Inscripcion')
END
GO

