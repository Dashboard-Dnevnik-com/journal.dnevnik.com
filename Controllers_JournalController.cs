using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class JournalController : Controller
{
    private static List<Student> students = Enumerable.Range(1, 20)
        .Select(i => new Student { Id = i, FullName = $"Ученик {i:00} Иванов" }).ToList();

    private static List<Lesson> lessons = Enumerable.Range(0, 17)
        .Select(i => new Lesson
        {
            Id = i + 1,
            Date = new DateTime(2023, 3, 31).AddDays(i * 2),
            Month = new DateTime(2023, 3, 31).AddDays(i * 2).ToString("MMMM", new System.Globalization.CultureInfo("ru-RU"))
        }).ToList();

    private static List<StudentMark> marks = new List<StudentMark>();

    public IActionResult Index()
    {
        var model = new JournalViewModel
        {
            Students = students,
            Lessons = lessons,
            Marks = marks,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult SetMark(int studentId, int lessonId, string value)
    {
        var mark = marks.FirstOrDefault(m => m.StudentId == studentId && m.LessonId == lessonId);
        value = value?.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            if (mark != null) marks.Remove(mark);
        }
        else
        {
            if (mark != null) mark.Value = value;
            else marks.Add(new StudentMark { StudentId = studentId, LessonId = lessonId, Value = value });
        }
        return Json(new { success = true });
    }
}
