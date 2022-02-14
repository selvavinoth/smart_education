$(document).ready(function() {

	$('#create_student_submit').click(function() {
		var student_name = $('#name').val();
		var birthday = $('#birthday').val();
		var sex = $('#sex').val();
		var bloodgroup = $('#bloodgroup').val();
		var religion = $('#religion').val();
		var address = $('#address').val();
		var phone = $('#phone').val();
		var email = $('#email').val();
		var father_name = $('#father_name').val();
		var mother_name = $('#mother_name').val();
		var edit_class = $('#edit_class').val();	
		var roll = $('#roll').val();
		var image = $('#image').val();

//echo "<script type='text/javascript'>alert('You may have not appropriate permission. Because you using Demo Version !');</script>";

		// alert(birthday);

		if (student_name == "" || birthday == "" || sex == "" || bloodgroup == "" || religion == "" || address == "" || phone == "" || email == "" || father_name == "" || mother_name == "" || edit_class == "" || roll == "" || image == "") {
			alert("fill All Field!");
		} else {
			var formData = new FormData($('#signupForm')[0]);

		    $.ajax({
		        url: 'php/add_student.php',
		        type: 'POST',
		        data: formData,
		        async: false,
		        success: function (data) {
		        	if(data=='submit success') {
					alert(data);
					window.location.href='student_list.php';
					} else {
						alert(data);
						window.location.href='create_student.php';
					}
	                return false;
		            // alert(data)
		        },
		        cache: false,
		        contentType: false,
		        processData: false
		    });
		    // return false;
		    // window.location.href='student_list.php';
		}
		
	
		

	});

	$('#create_teacher_submit').click(function() {
		var teacher_name = $('#teacher_name').val();
		var birthday = $('#birthday').val();
		var sex = $('#sex').val();
		var bloodgroup = $('#bloodgroup').val();
		var religion = $('#religion').val();
		var address = $('#address').val();
		var phone = $('#phone').val();
		var email = $('#email').val();
		var image = $('#image').val();


		// alert(birthday);

		if (teacher_name == "" || birthday == "" || sex == "" || bloodgroup == "" || religion == "" || address == "" || phone == "" || email == "" || image == "") {
			alert("fill All Field!");
		} else {

			var formData = new FormData($('#signupForm_teacher')[0]);

		    $.ajax({
		        url: 'php/add_teacher.php',
		        type: 'POST',
		        data: formData,
		        async: false,
		        success: function (data) {
		            if(data=='submit success!') {
					alert(data);
					window.location.href='teacher_list.php';
					} else {
						alert(data);
						window.location.href='create_teacher.php';
					}
	                return false;
		        },
		        cache: false,
		        contentType: false,
		        processData: false
		    });
		    // return false;
		}
		
	
		

	});

	$('#create_subject_submit').click(function() {
		var subject_name = $('#name').val();
		var select_class = $('#select_class').val();
		var select_teacher = $('#select_teacher').val();

		// alert(birthday);

		if (subject_name == "" || select_class == "" || select_teacher == "" ) {
			alert("fill All Field!");
		} else {
			$.post('php/add_subject.php', { subject_name : subject_name, select_class : select_class, select_teacher : select_teacher }, function(data) {
				alert(data);
				window.location.href='subject_list.php';
                return false;
			});
		}
	
	});

	$('#create_class_submit').click(function() {
		var class_name = $('#name').val();
		var numaric_class_name = $('#num_name').val();
		var select_teacher = $('#select_teacher').val();

		// alert(select_teacher);

		if (class_name == "" || numaric_class_name == "" || select_teacher == "" ) {
			alert("fill All Field!");
		} else {
			$.post('php/add_class.php', { class_name : class_name, numaric_class_name : numaric_class_name, select_teacher : select_teacher }, function(data) {
				alert(data);
				window.location.href='class_list.php';
                return false;
			});
		}
	
	});  

	$('#create_exam_submit').click(function() {
		var exam_name = $('#name').val();
		var date = $('#date').val();
		var comment = $('#comment').val();

		if (exam_name == "" || date == "" ) {
			alert("fill All Field!");
		} else {
			$.post('php/add_exam.php', { exam_name : exam_name, date : date, comment : comment }, function(data) {
				if(data=='submit success!') {
				alert(data);
				window.location.href='exam_list.php';
				} else {
					alert(data);
					window.location.href='create_exam.php';
				}
                return false;
			});
		}
	
	});


	$('#create_grade_submit').click(function() {
		var grade_name = $('#name').val();
		var point = $('#point').val();
		var mark_from = $('#mark-from').val();
		var mark_upto = $('#mark-upto').val();
		var comment = $('#comment').val();

		if (grade_name == "" || point == "" || mark_from == "" || mark_upto =="" ) {
			alert("fill All Field!");
		} else {
			$.post('php/add_grade.php', { grade_name : grade_name, point : point, mark_from : mark_from, mark_upto : mark_upto, comment : comment }, function(data) {
				if(data=='submit success!') {
				alert(data);
				window.location.href='grade_list.php';
				} else {
					alert(data);
					window.location.href='create_grade.php';
				}
                return false;
			});
		}
	
	});

	













	

});