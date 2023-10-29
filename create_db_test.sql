CREATE TABLE questions
(
 id INTEGER PRIMARY KEY,
 text TEXT
);
INSERT INTO questions VALUES(1,'Hungarian?');
INSERT INTO questions VALUES(2,'Living?'   );
INSERT INTO questions VALUES(3,'Male?'     );
INSERT INTO questions VALUES(4,'Writer?'   );

CREATE TABLE reactions
(
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 name VARCHAR(10),
 text TEXT,
 value REAL
);
INSERT INTO reactions VALUES(1,'Yes',      'Yes.',          4);
INSERT INTO reactions VALUES(2,'MaybeYes', 'Maybe yes.',    3);
INSERT INTO reactions VALUES(3,'IDontKnow','I don''t know.',2);
INSERT INTO reactions VALUES(4,'MaybeNo',  'Maybe no.',     1);
INSERT INTO reactions VALUES(5,'No',       'No.',           0);

CREATE TABLE solutions
(
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 text TEXT
);
INSERT INTO solutions VALUES( 1,'Háy'     );
INSERT INTO solutions VALUES( 2,'Lator'   );
INSERT INTO solutions VALUES( 3,'Schäffer');
INSERT INTO solutions VALUES( 4,'Tóth'    );
INSERT INTO solutions VALUES( 5,'Jókai'   );
INSERT INTO solutions VALUES( 6,'Petőfi'  );
INSERT INTO solutions VALUES( 7,'Kaffka'  );
INSERT INTO solutions VALUES( 8,'Psyché'  );
INSERT INTO solutions VALUES( 9,'Allen'   );
INSERT INTO solutions VALUES(10,'Dylan'   );
INSERT INTO solutions VALUES(11,'Rowling' );
INSERT INTO solutions VALUES(12,'Atwood'  );
INSERT INTO solutions VALUES(13,'Twain'   );
INSERT INTO solutions VALUES(14,'Villon'  );
INSERT INTO solutions VALUES(15,'Christie');
INSERT INTO solutions VALUES(16,'Barrett' );

CREATE TABLE domains
(
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 name TEXT
);
INSERT INTO domains VALUES(1,'Characters');

CREATE TABLE problems
(
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 domain_id INTEGER,
 solution_id INTEGER,
 FOREIGN KEY (domain_id) REFERENCES domains (id) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (solution_id) REFERENCES solutions (id) ON DELETE CASCADE ON UPDATE CASCADE
);
INSERT INTO problems VALUES( 1,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Háy'     ));
INSERT INTO problems VALUES( 2,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Lator'   ));
INSERT INTO problems VALUES( 3,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Schäffer'));
INSERT INTO problems VALUES( 4,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Tóth'    ));
INSERT INTO problems VALUES( 5,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Jókai'   ));
INSERT INTO problems VALUES( 6,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Petőfi'  ));
INSERT INTO problems VALUES( 7,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Kaffka'  ));
INSERT INTO problems VALUES( 8,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Psyché'  ));
INSERT INTO problems VALUES( 9,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Allen'   ));
INSERT INTO problems VALUES(10,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Dylan'   ));
INSERT INTO problems VALUES(11,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Rowling' ));
INSERT INTO problems VALUES(12,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Atwood'  ));
INSERT INTO problems VALUES(13,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Twain'   ));
INSERT INTO problems VALUES(14,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Villon'  ));
INSERT INTO problems VALUES(15,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Christie'));
INSERT INTO problems VALUES(16,(SELECT id FROM domains WHERE name='characters'),(SELECT id FROM solutions WHERE text='Barrett' ));

CREATE INDEX problems_idx_domain ON problems(domain_id);
CREATE INDEX problems_idx_solution ON problems(solution_id);

CREATE TABLE details
(
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 problem_id INTEGER,
 question_id INTEGER,
 reaction_id INTEGER,
 FOREIGN KEY (problem_id) REFERENCES problems (id) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (question_id) REFERENCES questions (id) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (reaction_id) REFERENCES reactions (id) ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO details VALUES( 1, 1/*Háy     */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 2, 1/*Háy     */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 3, 1/*Háy     */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 4, 1/*Háy     */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES( 5, 2/*Lator   */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 6, 2/*Lator   */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 7, 2/*Lator   */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES( 8, 2/*Lator   */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No'));

INSERT INTO details VALUES( 9, 3/*Schäffer*/,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(10, 3/*Schäffer*/,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(11, 3/*Schäffer*/,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(12, 3/*Schäffer*/,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(13, 4/*Tóth    */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(14, 4/*Tóth    */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(15, 4/*Tóth    */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(16, 4/*Tóth    */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(17, 5/*Jókai   */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(18, 5/*Jókai   */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(19, 5/*Jókai   */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(20, 5/*Jókai   */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(21, 6/*Petőfi  */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(22, 6/*Petőfi  */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(23, 6/*Petőfi  */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(24, 6/*Petőfi  */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(25, 7/*Kaffka  */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(26, 7/*Kaffka  */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(27, 7/*Kaffka  */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(28, 7/*Kaffka  */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(29, 8/*Psyché  */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(30, 8/*Psyché  */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(31, 8/*Psyché  */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(32, 8/*Psyché  */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(33, 9/*Allen   */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(34, 9/*Allen   */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(35, 9/*Allen   */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(36, 9/*Allen   */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(37,10/*Dylan   */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(38,10/*Dylan   */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(39,10/*Dylan   */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(40,10/*Dylan   */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(41,11/*Rowling*/,(SELECT id FROM questions WHERE text='Hungarian?' ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(42,11/*Rowling*/,(SELECT id FROM questions WHERE text='Living?'    ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(43,11/*Rowling*/,(SELECT id FROM questions WHERE text='Male?'      ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(44,11/*Rowling*/,(SELECT id FROM questions WHERE text='Writer?'    ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(45,12/*Atwood */,(SELECT id FROM questions WHERE text='Hungarian?' ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(46,12/*Atwood */,(SELECT id FROM questions WHERE text='Living?'    ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(47,12/*Atwood */,(SELECT id FROM questions WHERE text='Male?'      ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(48,12/*Atwood */,(SELECT id FROM questions WHERE text='Writer?'    ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(49,13/*Twain   */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(50,13/*Twain   */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(51,13/*Twain   */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(52,13/*Twain   */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(53,14/*Villon  */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(54,14/*Villon  */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(55,14/*Villon  */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='Yes'));
INSERT INTO details VALUES(56,14/*Villon  */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

INSERT INTO details VALUES(57,15/*Christie*/,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(58,15/*Christie*/,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(59,15/*Christie*/,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(60,15/*Christie*/,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='Yes'));

INSERT INTO details VALUES(61,16/*Barrett */,(SELECT id FROM questions WHERE text='Hungarian?'),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(62,16/*Barrett */,(SELECT id FROM questions WHERE text='Living?'   ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(63,16/*Barrett */,(SELECT id FROM questions WHERE text='Male?'     ),(SELECT id FROM reactions WHERE name='No' ));
INSERT INTO details VALUES(64,16/*Barrett */,(SELECT id FROM questions WHERE text='Writer?'   ),(SELECT id FROM reactions WHERE name='No' ));

.save BARKOCHBA.sqlite
.quit
